using System;
using System.Collections.Generic;
using Snake3D;
using System.IO;
using UnityEngine;
using System.Net.NetworkInformation;
using System.Collections;
using System.Xml;

public class MapData
{
    public Int32 id;
    public Int32 mapWidth;
    public Int32 mapHeight;
    public Int32 camHeight;
    public Int32 camOffset;
}

[Module("GameMudule", true)]
public class GameMudule : ModuleBase
{
	public MsgRoomEnter roomEnterData;
	public MsgAddTargetPos AddTargetPos;
	public List<Snake> m_OtherSnakeObj;
	public Snake m_SelfSnake;
	public string UserNmae;
    public bool isInRoom;
    
	private Vector3 m_ToDirection;
	private GameView m_GameView;
    private Dictionary<int,MapData> mapDataDic = new Dictionary<int, MapData>();
    private MapData curMapData = new MapData();

    public override void OnLoad()
    {
        ReadMapXML();
        NetManager.Instance.AddNetCallback("MsgMsgInit",OnNetMsgInit);
		NetManager.Instance.AddNetCallback("MsgLogin", OnLogin);
		NetManager.Instance.AddNetCallback("MsgRoomInfo", OnNetGetRoomInfo);
		NetManager.Instance.AddNetCallback("MsgRoomEnter",NetEnterRoom);
		NetManager.Instance.AddNetCallback("MsgAddTargetPos",MoveToNewPositaion);
        NetManager.Instance.Connect();
        GetMsgConfig();
    }

    public override void OnRelease()
    {
        NetManager.Instance.RemoveNetCallback("MsgMsgInit", OnNetMsgInit);
        NetManager.Instance.RemoveNetCallback("MsgRoomInfo", OnNetGetRoomInfo);
        NetManager.Instance.RemoveNetCallback("MsgLogin", OnLogin);
		NetManager.Instance.RemoveNetCallback("MsgRoomEnter",NetEnterRoom);
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
    }

    public void SendToEnterRoom()
    {
        MsgRoomEnter msgEnter = new MsgRoomEnter();
		msgEnter.AccountId = UserNmae;
        msgEnter.RoomId = 1;
        NetManager.Instance.SendMessage("MsgRoomEnter", msgEnter);
    }
	void NetEnterRoom(object msg)
	{
		roomEnterData = msg as MsgRoomEnter;
        isInRoom = true;
         curMapData = mapDataDic[1];
        Notification notify = new Notification("MapConfig", null);
        notify["MapConfigs"] = curMapData;
        notify.Send();
        // Debug.Log("22222222222222222222222222222222222222222222222");
        InitSnake(roomEnterData);
    }

    public MapData GetCurMapData()
    {
        return curMapData;
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

    public MapData GetMapData(int id)
    {
        return mapDataDic[id];
    }

    public bool IsInRoom
    {
        get { return isInRoom; }
    }
	private void MoveToNewPositaion(object msg){
        if (!isInRoom) return;
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
       // for (int i = 0; i < roomEnterData.PlayerList.Count; i++)
        {
            MsgPlayerInfo info = roomEnterData.PlayerList[0];
            Vector3 StartVector = new Vector3(info.DirectionX, 0, info.DirectionY);
            m_SelfSnake = CreateSnake(info.AccountId + "Snake", StartVector, info.SurplusLength, info.Speed);
            m_ToDirection = new Vector3(info.DirectionX, 0, info.DirectionY);
        }
    }
	// Update is called once per frame
	public Snake CreateSnake(string name,Vector3 pos,UInt32 SetSelfLength,float speed)
	{
		Snake snake = new Snake();
		snake.Init(name, pos,SetSelfLength,speed);
        m_SelfSnake = snake;
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

    private void ReadMapXML()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("Assets/Resources/Configs/Mapconfig.xml");
        XmlNode rootNode = xmlDoc.FirstChild.NextSibling;
        for (int i = 0; i < rootNode.ChildNodes.Count; i++)
        {
            XmlNode node = rootNode.ChildNodes[i];
            MapData map = new MapData();
           // < map id = "1" width = "500" length = "500" path = "map/Map1" camHeight = "30" camOffet = "-10" maxPlayer = "50" />
            map.id = Int32.Parse(node.Attributes["id"].Value);
            map.mapWidth = Int32.Parse(node.Attributes["width"].Value);
            map.mapHeight = Int32.Parse(node.Attributes["length"].Value);
            map.camOffset = Int32.Parse(node.Attributes["camOffet"].Value);
            map.camHeight = Int32.Parse(node.Attributes["camHeight"].Value);
            mapDataDic.Add(map.id, map);
           // Debug.Log("嗯呢"+node.Attributes["width"].Value);
        }
       
    }

  
}
