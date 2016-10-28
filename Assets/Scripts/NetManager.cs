using System;
using System.Reflection;
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
    private ProtobufSerializer serializer;

    private NetManager() {
        serializer = new ProtobufSerializer();
    }

    public void Update()
    {
        int count = m_callMap.Count;
        while(count-- > 0)
        {
            NetCall call = m_callMap[count];
            for(int i = 0; i < call.calls.Count; i++)
            {
                call.calls[i].Invoke(call.msg);
            }
            m_callMap.RemoveAt(count);
            count = m_callMap.Count;
        }
    }

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
            connected = true;
            m_stream = m_tcpClient.GetStream();
            lock (m_stream)
            {
                AsyncCallback callBack = new AsyncCallback(ReadComplete);
                m_stream.BeginRead(m_buff, 0, BUFF_SIZE, callBack, null);
            }
        }
    }

    public void DisConnect()
    {
        if (m_stream != null)
            m_stream.Dispose();
        m_tcpClient.Close();
    }

    public void AddNetCallback(string key,Action<object> callback)
    {
        //Int16 id = NetIDContainer.GetMessageId(key);
        if(m_netCallbackMap.ContainsKey(key))
        {
            List<Action<object>> callbacks = m_netCallbackMap[key];
            callbacks.Add(callback);
        }
        else
        {
            List<Action<object>> callbacks = new List<Action<object>>();
            callbacks.Add(callback);
            m_netCallbackMap.Add(key, callbacks);
        }
    }

    public void RemoveNetCallback(string key, Action<object> callback)
    {
      //  Int16 id = NetIDContainer.GetMessageId(key);
        if (m_netCallbackMap.ContainsKey(key) && m_netCallbackMap[key].Contains(callback))
        {
            List<Action<object>> callbacks = m_netCallbackMap[key];
            callbacks.Remove(callback);
        }
    }

    public bool Connected
    {
        get { return connected; }
    }

    public void SendMessage(string key,object msg)
    {
        if(!connected)
        {
            Debug.LogError("没有连接到服务器！");
            return;
        }

        byte[] buff = null;
        MemoryStream stream = new MemoryStream();
        serializer.Serialize(stream, msg);
        buff = stream.ToArray();

        MemoryStream strm = new MemoryStream();
        byte[] msgLenBytes = System.BitConverter.GetBytes((Int16)(buff.Length + 2));
        strm.Write(msgLenBytes, 0, msgLenBytes.Length);
        byte[] msgIdBytes = System.BitConverter.GetBytes(NetIDContainer.GetMessageId(key));
        Debug.Log("=======发送：" + key);
        strm.Write(msgIdBytes, 0, msgIdBytes.Length);
        strm.Write(buff, 0, buff.Length);

        byte[] toSendBytes = strm.ToArray();
        m_stream.Write(toSendBytes, 0, toSendBytes.Length);
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
            Int16 msgLen = BitConverter.ToInt16(GetBuff(m_buff, 0, 2),0);
            Int16 msgId = BitConverter.ToInt16(GetBuff(m_buff,2, 2), 0);
            string msgKey = NetIDContainer.GetMessageKey(msgId);

            Debug.Log("=======接收ID: " + msgId + "  key:" + msgKey);
            MemoryStream msgStream = new MemoryStream();
            msgStream.Write(m_buff, 4, msgLen - 2);
            msgStream.Position = 0;
            Type type = Assembly.GetAssembly(typeof(MsgMsgInit)).GetType("Snake3D." + msgKey, true);
            if (null == type)
            {
                Debug.Log("没有类名是： Snake3D." + msgKey + "的类！");
                return;
            }
            ProtobufSerializer serializer = new ProtobufSerializer();
            object msg = serializer.Deserialize(msgStream,null,type);
            InvokeCallback(msgKey, msg);
			if("MsgError" == msgKey)
			{
				MsgError err = msg as MsgError;
				Debug.Log("========MsgError=======" + err.ErrorIdx);
			}
            Array.Clear(m_buff, 0, m_buff.Length);      // 清空缓存，避免脏读
            lock (m_stream)
            {
                AsyncCallback callBack = new AsyncCallback(ReadComplete);
                m_stream.BeginRead(m_buff, 0, BUFF_SIZE, callBack, null);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("===============: " + ex.Message);
            if (m_stream != null)
                m_stream.Dispose();
            m_tcpClient.Close();
        }
    }

    private void InvokeCallback(string msgKey,object msg)
    {
        if(m_netCallbackMap.ContainsKey(msgKey))
        {
            List<Action<object>> callbacks = m_netCallbackMap[msgKey];
            NetCall netCall = new NetCall();
            netCall.msgKey = msgKey;
            netCall.msg = msg;
            netCall.calls = callbacks;
            m_callMap.Add(netCall);
            //int count = callbacks.Count;
            //while(count-- > 0)
            //{
            //    Action<object> callback = callbacks[count];
            //    callback.Invoke(msg);
            //}
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

    private bool connected = false;
    private TcpClient m_tcpClient;
    private NetworkStream m_stream;
    private byte[] m_buff = new byte[BUFF_SIZE];
    private const int BUFF_SIZE= 8192;
    private Dictionary<string, List<Action<object>>> m_netCallbackMap = new Dictionary<string, List<Action<object>>>();
    private List<NetCall> m_callMap = new List<NetCall>();
}

public class NetCall
{
    public string msgKey;
    public object msg;
    public List<Action<object>> calls;
}
