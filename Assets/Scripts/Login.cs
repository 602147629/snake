using UnityEngine;
using System.Collections;
using Snake3D;
using System.Net.Sockets;
using System.Net;
using ProtoBuf;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System;

public class Login : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 翻转字节顺序 (32-bit)
    public static UInt32 ReverseBytes(UInt32 value)
    {
        return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
               (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
    }

    void OnGUI()
    {
        if (GUILayout.Button("登录"))
        {
            NetManager.Instance.Connect();
            MsgMsgInit msgInit = new MsgMsgInit();
            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize<MsgMsgInit>(stream, msgInit);
            NetManager.Instance.SendMessage("MsgMsgInit", stream.ToArray());

            //Snake3D.Login loginMsg = new Snake3D.Login();
            //loginMsg.AccountId = "meizu";
            //loginMsg.ThemeType = 1;
            //using (TcpClient client = new TcpClient())
            //{
            //    client.Connect(IPAddress.Parse("192.168.9.200"), 3563);
            //    Debug.Log("CLIENT: socket 连接成功...");

            //    using (NetworkStream stream = client.GetStream())
            //    {
            //        //发送
            //        Debug.Log("CLIENT : 发送数据...");
            //        using (MemoryStream mstream = new MemoryStream())
            //        {
            //            ProtoBuf.Serializer.Serialize<Snake3D.Login>(mstream, loginMsg);
            //            mstream.Position = 0;

            //            MemoryStream ms = new MemoryStream();
            //            byte[] lens = System.BitConverter.GetBytes((Int16)(mstream.Length+2));
            //            ms.Write(lens, 0, lens.Length);
            //            byte[] lenId = System.BitConverter.GetBytes((Int16)0);
            //            ms.Write(lenId, 0, lenId.Length);
            //            byte[] b = mstream.ToArray();
            //            ms.Write(b, 0, b.Length);
            //            byte[] s = ms.ToArray();
            //            stream.Write(s, 0, s.Length);
            //        }

            //        //接收
            //        Debug.Log("CLIENT : 等待响应...");
            //        byte[] buff = new byte[4096];
            //        int readLen = stream.Read(buff, 0, 4096);
            //        byte[] dataLen = new byte[2] {  buff[0],buff[1] };
            //        int msgLen = BitConverter.ToInt16(new byte[2] { buff[0], buff[1] }, 0);
            //        int msgId = BitConverter.ToInt16(new byte[2] { buff[2], buff[3] }, 0);
            //        MemoryStream msgStream = new MemoryStream();
            //        msgStream.Write(buff, 4, msgLen-2);
            //        msgStream.Position = 0;
            //        Snake3D.Login dData = ProtoBuf.Serializer.Deserialize<Snake3D.Login>(msgStream);
                       
            //        string result = string.Format("CLIENT: 成功获取结果, RoomId ={0}, RoomW ={1}, RoomH ={2}, StartX ={3}, StartY ={4}", dData.RoomId, dData.RoomW, dData.RoomH, dData.StartX, dData.StartY);
            //        Debug.Log(result);
            //        //关闭
            //        stream.Close();
            //    }
            //    client.Close();
            //    Debug.Log("CLIENT : 关闭...");
            }
        }
    }
