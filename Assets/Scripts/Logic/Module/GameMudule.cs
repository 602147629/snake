using System;
using System.Collections.Generic;
using Snake3D;
using System.IO;
using UnityEngine;

[Module("GameMudule", true)]
public class GameMudule : ModuleBase
{
	public MsgRoomEnter roomEnterData;
	public List<Snake> m_OtherSnakeObj;
	public Snake m_SelfSnake;
	private Vector3 m_ToDirection;
	private GameView m_GameView;

    public override void OnLoad()
    {
        NetManager.Instance.AddNetCallback("MsgMsgInit",OnNetMsgInit);
        NetManager.Instance.Connect();
        GetMsgConfig();
        
    }

    public override void OnRelease()
    {
        NetManager.Instance.RemoveNetCallback("MsgMsgInit", OnNetMsgInit);
        NetManager.Instance.RemoveNetCallback("MsgRoomInfo", OnNetGetRoomInfo);
        NetManager.Instance.RemoveNetCallback("MsgLogin", OnLogin);
		NetManager.Instance.RemoveNetCallback("MsgRoomEnter",OnGetMessageRoomBack);
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
        NetManager.Instance.AddNetCallback("MsgLogin", OnLogin);
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
        NetManager.Instance.AddNetCallback("MsgRoomInfo", OnNetGetRoomInfo);
        MsgRoomInfo roomInfo = new MsgRoomInfo();
        NetManager.Instance.SendMessage("MsgRoomInfo", roomInfo);
    }

    void OnNetGetRoomInfo(object msg)
    {
        MsgRoomInfo roomInfo = msg as MsgRoomInfo;
    }

    public void SendToEnterRoom()
    {
		NetManager.Instance.AddNetCallback("MsgRoomEnter",OnGetMessageRoomBack);
        MsgRoomEnter msgEnter = new MsgRoomEnter();
        msgEnter.AccountId = "meizu";
        msgEnter.RoomId = 1;
        NetManager.Instance.SendMessage("MsgRoomEnter", msgEnter);
    }
	void OnGetMessageRoomBack(object msg)
	{
		roomEnterData = msg as MsgRoomEnter;
	}


	public void MsgExitRoom(){
		MsgExitRoom msgExit = new Snake3D.MsgExitRoom ();
		NetManager.Instance.SendMessage("MsgExitRoom",msgExit);
	}


	public void Init(GameView gv)
	{
		m_GameView = gv;
		m_ToDirection = new Vector3(0, 0, 1);
	}
	
	// Update is called once per frame
	public void Update(float deltaTime)
	{
		this.m_SelfSnake.Move(m_ToDirection, deltaTime);
	}
	public Snake CreateSnake(string name,Vector3 pos,UInt32 SetSelfLength,float speed)
	{
		Snake snake = new Snake();
		snake.Init(name, pos,SetSelfLength,speed);
		return snake;
	}
	public void SetSelfTo(Vector3 to)
	{
		m_ToDirection = to;
	}
	public void SetSelfLength(UInt32 length)
	{
		m_SelfSnake.SetLength(length);
	}
}
