using UnityEngine;
using System.Collections;
using Snake3D;

public class GameView : MonoBehaviour {

    public Camera myCamera;
	private GameMudule gameMudule;
    // Use this for initialization
    void Start () {
		gameMudule = ModuleManager.Instance.GetModule<GameMudule> ();
		gameMudule.Init (this);
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
}
