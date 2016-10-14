using UnityEngine;
using System.Collections.Generic;
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
        NetManager.Instance.AddNetCallback("MsgMsgInit", Call);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Call(object data)
    {
        MsgMsgInit initMsg = data as MsgMsgInit;
        List<MsgMsgData> msgConfs = initMsg.MsgList;
        for(int i = 0; i < msgConfs.Count; i++)
        {
            MsgMsgData item = msgConfs[i];
            NetIDContainer.AddIdName((Int16)item.MsgId, item.MsgName);
            NetIDContainer.AddNameId(item.MsgName, (Int16)item.MsgId);
        }
    }

    void OnGUI()
    {
        if (GUILayout.Button("登录"))
        {
            if(!NetManager.Instance.Connected)
            {
                NetManager.Instance.Connect(GameConfig.serverIP);
            }
            MsgMsgInit msgInit = new MsgMsgInit();
            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize<MsgMsgInit>(stream, msgInit);
            NetManager.Instance.SendMessage("MsgMsgInit", stream.ToArray());
        }
    }
}
