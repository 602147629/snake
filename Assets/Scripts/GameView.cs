using UnityEngine;
using System.Collections;
using Snake3D;

public class GameView : MonoBehaviour {

    public Camera myCamera;
	private GameMudule gameMudule;
    // Use this for initialization
    void Start () {
		gameMudule = ModuleManager.Instance.GetModule<GameMudule>();
		gameMudule.Init(this);
		MsgRoomEnter EnterInfo = gameMudule.roomEnterData;
		if (EnterInfo == null) {
			return;
		} else {
			for (int i=0; i<EnterInfo.PlayerList.Count; i++) {
				Vector3 StartVector=new Vector3(EnterInfo.PlayerList[i].DirectionX,0,EnterInfo.PlayerList[i].DirectionY);
				gameMudule.m_SelfSnake = gameMudule.CreateSnake (EnterInfo.PlayerList[i].AccountId, StartVector,EnterInfo.PlayerList[i].SurplusLength,EnterInfo.PlayerList[i].Speed*0.005f);
//				gameMudule.m_SelfSnake = gameMudule.CreateSnake ("123", StartVector,10,1);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		gameMudule.m_SelfSnake.Update();
    }

    void LateUpdate()
    {
		Vector3 tarPos = gameMudule.m_SelfSnake._nodeList[0].transform.position;
        myCamera.transform.position = tarPos + new Vector3(0, 20, -10);
        myCamera.transform.LookAt(tarPos);
    }
}
