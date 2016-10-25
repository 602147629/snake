using System;
using System.Collections.Generic;
using Snake3D;
using System.IO;
using UnityEngine;

[Module("GameMudule", true)]
public class GameMudule : ModuleBase
{
    public override void OnLoad()
    {
        NetManager.Instance.AddNetCallback("MsgMsgInit",OnNetMsgInit);
        NetManager.Instance.Connect();
        GetMsgConfig();
        
    }

    public override void OnRelease()
    {
        NetManager.Instance.RemoveNetCallback("MsgMsgInit", OnNetMsgInit);
        NetManager.Instance.RemoveNetCallback("snake.MsgRoomInfo", OnNetGetRoomInfo);
        NetManager.Instance.RemoveNetCallback("snake.MsgLogin", OnLogin);
    }

    void GetMsgConfig()
    {
        MsgMsgInit msgInit = new MsgMsgInit();
        NetManager.Instance.SendMessage("MsgMsgInit", msgInit);
    }

    void OnNetMsgInit(object msg)
    {
        MsgMsgInit initMsg = msg as MsgMsgInit;
        List<MsgMsgData> msgConfs = initMsg.MsgList;
        for (int i = 0; i < msgConfs.Count; i++)
        {
            MsgMsgData item = msgConfs[i];
            NetIDContainer.AddIdName((Int16)item.MsgId, item.MsgName);
            NetIDContainer.AddNameId(item.MsgName, (Int16)item.MsgId);
        }
        Login();
    }

    void Login()
    {
        NetManager.Instance.AddNetCallback("snake.MsgLogin", OnLogin);
        MsgLogin msgLogin = new MsgLogin();
        msgLogin.AccountId = "meizu";
        NetManager.Instance.SendMessage("MsgLogin", msgLogin);
    }

    void OnLogin(object msg)
    {
        GetRoomInfo();
    }

    void GetRoomInfo()
    {
        NetManager.Instance.AddNetCallback("snake.MsgRoomInfo", OnNetGetRoomInfo);
        MsgRoomInfo roomInfo = new MsgRoomInfo();
        NetManager.Instance.SendMessage("snake.MsgRoomInfo", roomInfo);
    }

    void OnNetGetRoomInfo(object msg)
    {
        MsgRoomInfo roomInfo = msg as MsgRoomInfo;
    }

    public void SendToEnterRoom()
    {
        MsgRoomEnter msgEnter = new MsgRoomEnter();
        msgEnter.AccountId = "meizu";
        msgEnter.RoomId = 1;
        NetManager.Instance.SendMessage("snake.MsgRoomEnter", msgEnter);
    }
}
