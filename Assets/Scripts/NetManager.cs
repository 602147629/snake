using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Snake3D;
/// <summary>
/// 利用 .net 的Tcp与服务器（go）通信
/// 消息格式
/// ----------------
/// |len|id|message|
/// ----------------
/// len：两个字节，表示id+message的长度
/// id：两个字节，表示该消息的消息号
/// message：字节数不定，表示消息内容，使用protobuf序列化的内容
/// </summary>
public class NetManager
{
    public static readonly NetManager Instance = new NetManager();

    private NetManager() { }

    public void Connect(string ip = GameConfig.serverIP,int port = GameConfig.serverPort)
    {
        if(null != m_tcpClient)
        {
            m_tcpClient.Close();
        }
        else
        {
            try
            {
                m_tcpClient = new TcpClient();
                m_tcpClient.Connect(IPAddress.Parse(ip), port);
            }
            catch(Exception ex)
            {
                Debug.Log("====连接" + ip + ":" + port + "失败！");
                Debug.Log("失败信息： " + ex.Message);
                return;
            }
            Debug.Log("====成功连接到服务器" + ip + ":" + port);
            m_stream = m_tcpClient.GetStream();
        }
    }

    public void AddNetCallback(string key,Action<object> callback)
    {
        Int16 id = NetIDContainer.GetMessageId(key);
        if(m_netCallbackMap.ContainsKey(id))
        {
            List<Action<object>> callbacks = m_netCallbackMap[id];
            callbacks.Add(callback);
        }
        else
        {
            List<Action<object>> callbacks = new List<Action<object>>();
            callbacks.Add(callback);
            m_netCallbackMap.Add(id, callbacks);
        }
    }

    public void RemoveNetCallback(string key, Action<object> callback)
    {
        Int16 id = NetIDContainer.GetMessageId(key);
        if (m_netCallbackMap.ContainsKey(id) && m_netCallbackMap[id].Contains(callback))
        {
            List<Action<object>> callbacks = m_netCallbackMap[id];
            callbacks.Remove(callback);
        }
    }

    public void SendMessage(string key,byte[] buff)
    {
        MemoryStream strm = new MemoryStream();
        byte[] msgLenBytes = System.BitConverter.GetBytes((Int16)(buff.Length + 2));
        strm.Write(msgLenBytes, 0, msgLenBytes.Length);
        byte[] msgIdBytes = System.BitConverter.GetBytes(NetIDContainer.GetMessageId(key));
        strm.Write(msgLenBytes, 0, msgLenBytes.Length);
        strm.Write(buff, 0, buff.Length);

        byte[] toSendBytes = strm.ToArray();
        m_stream.Write(toSendBytes, 0, toSendBytes.Length);

        lock(m_stream)
        {
            AsyncCallback callBack = new AsyncCallback(ReadComplete);
            m_stream.BeginRead(m_buff, 0, BUFF_SIZE, callBack, null);
        }
    }

    private void ReadComplete(IAsyncResult ar)
    {
        int bytesRead;

        try
        {
            lock (m_stream)
            {
                bytesRead = m_stream.EndRead(ar);
            }
            if (bytesRead == 0)
            {
                return;
                //throw new Exception("读取到0字节");
            }
            Int16 msgLen = BitConverter.ToInt16(GetBuff(m_buff, 0, 2),0);
            Int16 msgId = BitConverter.ToInt16(GetBuff(m_buff,2, 2), 0);
            string msgKey = NetIDContainer.GetMessageKey(msgId);
            MemoryStream msgStream = new MemoryStream();
            msgStream.Write(m_buff, 4, msgLen - 2);
            msgStream.Position = 0;
            Type type = Type.GetType(msgKey);
            ProtobufSerializer serializer = new ProtobufSerializer();
            object msg = serializer.Deserialize(msgStream,null,type);
            InvokeCallback(msgId, msg);

            Array.Clear(m_buff, 0, m_buff.Length);      // 清空缓存，避免脏读
            lock (m_stream)
            {
                AsyncCallback callBack = new AsyncCallback(ReadComplete);
                m_stream.BeginRead(m_buff, 0, BUFF_SIZE, callBack, null);
            }
        }
        catch (Exception ex)
        {
            if (m_stream != null)
                m_stream.Dispose();
            m_tcpClient.Close();
        }
    }

    private void InvokeCallback(Int16 msgId,object msg)
    {
        if(m_netCallbackMap.ContainsKey(msgId))
        {
            List<Action<object>> callbacks = m_netCallbackMap[msgId];
            int count = callbacks.Count;
            while(count-- > 0)
            {
                Action<object> callback = callbacks[count];
                callback.Invoke(msg);
            }
        }
    }

    private  byte[] GetBuff(byte[] buff,int offset,int count)
    {
        byte[] retBuff = new byte[count];
        for(int i = 0; i < count; i++)
        {
            retBuff[i] = buff[i + offset];
        }
        return retBuff;
    }

    private TcpClient m_tcpClient;
    private NetworkStream m_stream;
    private byte[] m_buff;
    private const int BUFF_SIZE= 8192;
    private Dictionary<Int16, List<Action<object>>> m_netCallbackMap;
}
