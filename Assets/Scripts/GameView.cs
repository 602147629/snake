using System;
using UnityEngine;
using System.Collections;
using Snake3D;
using UnityEngine.Networking;

public class GameView : MonoBehaviour
{
    public GameObject mMap;
    public Camera myCamera;
	private GameMudule gameMudule;
    // Use this for initialization
    void Start () {
		gameMudule = ModuleManager.Instance.GetModule<GameMudule> ();
		gameMudule.Init (this);
	}
    void OnEnable()
    {
        Messager.Instance.AddNotification("readMapConfig", mapConfig);
    }
    // Update is called once per frame
    void Update () {
		if (gameMudule.m_SelfSnake == null) {
			return;
		}
		gameMudule.m_SelfSnake.Update();
    }

    void LateUpdate()
    {
		if (gameMudule.m_SelfSnake == null) {
			return;
		}
		Vector3 tarPos = gameMudule.m_SelfSnake._nodeList[0].transform.position;
        myCamera.transform.position = tarPos + new Vector3(0, 20, -10);
        myCamera.transform.LookAt(tarPos);
    }

    private void mapConfig(Notification msg)
    {
        Int32 id = (Int32)msg["configId"];
        MapData mapData = gameMudule.GetMapData(id);
        mMap.transform.localScale= new Vector3(mapData.mapWidth,1,mapData.mapHeight);
    }
}
