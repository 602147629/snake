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
	private Vector3 camOffset;
    // Use this for initialization
    void Start () {
		gameMudule = ModuleManager.Instance.GetModule<GameMudule> ();
		gameMudule.Init (this);
		mapConfig();
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
        myCamera.transform.position = tarPos + camOffset;
        myCamera.transform.LookAt(tarPos);
    }

    private void mapConfig()
    {
        MapData mapData = gameMudule.GetCurMapData();
        mMap.transform.localScale= new Vector3(mapData.mapWidth,1,mapData.mapHeight);
        camOffset = new Vector3(0,mapData.camHeight,mapData.camOffset);
    }
}
