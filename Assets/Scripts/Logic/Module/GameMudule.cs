using System;
using System.Collections.Generic;
using Snake3D;
using System.IO;
using UnityEngine;
using System.Net.NetworkInformation;
using System.Collections;

[Module("GameMudule", true)]
public class GameMudule : ModuleBase
{
	public MsgRoomEnter roomEnterData;
	public MsgAddTargetPos AddTargetPos;
	public List<Snake> m_OtherSnakeObj;
	public Snake m_SelfSnake;
	private Vector3 m_ToDirection;
	private GameView m_GameView;
	public string UserNmae;
    public override void OnLoad()
    {
	#if UNITY_ANDROID
	private AndroidJavaObject javaObject = null;
	private AndroidJavaClass javaClass = null;
	javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
	javaObject = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
	#endif
		UserNmae = GetMacAddressPCIOS ();
        NetManager.Instance.AddNetCallback("MsgMsgInit",OnNetMsgInit);
		NetManager.Instance.AddNetCallback("MsgLogin", OnLogin);
		NetManager.Instance.AddNetCallback("MsgRoomInfo", OnNetGetRoomInfo);
		NetManager.Instance.AddNetCallback("MsgRoomEnter",OnGetMessageRoomBack);
		NetManager.Instance.AddNetCallback("MsgAddTargetPos",MoveToNewPositaion);
        NetManager.Instance.Connect();
        GetMsgConfig();
        
    }

    public override void OnRelease()
    {
        NetManager.Instance.RemoveNetCallback("MsgMsgInit", OnNetMsgInit);
        NetManager.Instance.RemoveNetCallback("MsgRoomInfo", OnNetGetRoomInfo);
        NetManager.Instance.RemoveNetCallback("MsgLogin", OnLogin);
		NetManager.Instance.RemoveNetCallback("MsgRoomEnter",OnGetMessageRoomBack);
		NetManager.Instance.RemoveNetCallback("MsgAddTargetPos",MoveToNewPositaion);
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
        MsgLogin msgLogin = new MsgLogin();
		msgLogin.AccountId = UserNmae;
        NetManager.Instance.SendMessage("MsgLogin", msgLogin);
    }

    void OnLogin(object msg)
    {
        GetRoomInfo();
    }

    void GetRoomInfo()
    {
        MsgRoomInfo roomInfo = new MsgRoomInfo();
        NetManager.Instance.SendMessage("MsgRoomInfo", roomInfo);
    }

    void OnNetGetRoomInfo(object msg)
    {
        MsgRoomInfo roomInfo = msg as MsgRoomInfo;
        //FoodNetInit();
    }

    public void SendToEnterRoom()
    {
        MsgRoomEnter msgEnter = new MsgRoomEnter();
		msgEnter.AccountId = UserNmae;
        msgEnter.RoomId = 1;
        NetManager.Instance.SendMessage("MsgRoomEnter", msgEnter);
    }
	void OnGetMessageRoomBack(object msg)
	{
		roomEnterData = msg as MsgRoomEnter;
		InitSnake (roomEnterData);
	 

	}
	
	public void MsgExitRoom(){
		MsgExitRoom msgExit = new Snake3D.MsgExitRoom ();
		NetManager.Instance.SendMessage("MsgExitRoom",msgExit);
	}

	public void MsgMove(float x,float y){
		MsgMove Move = new MsgMove ();
		MsgPosInfo PosInfo = new MsgPosInfo ();
		PosInfo.PosX = x;
		PosInfo.PosY = y;
		Move.AccountId = UserNmae;
		Move.TargetPos =PosInfo;
		NetManager.Instance.SendMessage("MsgMove",Move);
	}
	private void MoveToNewPositaion(object msg){
		 AddTargetPos = msg as MsgAddTargetPos;
		List <MsgPosStruct> PosStruct = AddTargetPos.PosList;
		for (int i=0; i<PosStruct.Count; i++) {
			if(PosStruct[i].AccountId==UserNmae){
				Vector3 MsgDirection = new Vector3 (PosStruct [i].PosX, 0, PosStruct [i].PosY);
				this.m_SelfSnake.Move (MsgDirection);
			}
		}
	}
	//初始化蛇的信息
	public void Init(GameView gv)
	{
		m_GameView = gv;
	}

	private void InitSnake(MsgRoomEnter roomEnterData)
	{
		Snake snake = new Snake ();
		if (roomEnterData == null) {
			return;
		} else {
			for (int i=0; i<roomEnterData.PlayerList.Count; i++) {
				Vector3 StartVector = new Vector3 (roomEnterData.PlayerList [i].DirectionX, 0, roomEnterData.PlayerList [i].DirectionY);
				m_SelfSnake=CreateSnake(roomEnterData.PlayerList [i].AccountId, StartVector, roomEnterData.PlayerList [i].SurplusLength, roomEnterData.PlayerList [i].Speed * 0.005f);
				if(UserNmae==roomEnterData.PlayerList[i].AccountId){
					m_ToDirection=new Vector3(roomEnterData.PlayerList[i].DirectionX,0,roomEnterData.PlayerList[i].DirectionY);
				}
			}
		}
	}
	// Update is called once per frame
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

    //获取mac地址
    public static string GetMacAddressPCIOS()
	{
		string physicalAddress = "";  
				
		NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();  
				
		foreach (NetworkInterface adaper in nice)  
		{  
			if (adaper.Description == "en0")  
			{  
				physicalAddress = adaper.GetPhysicalAddress().ToString();  
				break;
			}  
			else  
			{  
				physicalAddress = adaper.GetPhysicalAddress().ToString();  
				if (physicalAddress == null) {
					#if UNITY_ANDROID
					physicalAddress=javaObject.Call< string >("GetDevId");
					#endif
				}else if (physicalAddress != "")  
					{  
						break;  
					};  
			} 
		}  
		return physicalAddress; 

		}  
}
