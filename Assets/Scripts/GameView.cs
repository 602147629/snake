using UnityEngine;
using System.Collections;

public class GameView : MonoBehaviour {

    public Camera myCamera;
    public GameObject m_SnakeObjModel;
    private float time;
    private Vector3 to;
    // Use this for initialization
    void Start () {
        GameLogin.instance.Init(this);
        GameLogin.instance.m_SelfSnake = GameLogin.instance.CreateSnake("asdfasdfas");
        to = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }
	
	// Update is called once per frame
	void Update () {
        GameLogin.instance.Update();

        Vector3 tarPos = GameLogin.instance.m_SelfSnake._nodeList[0].transform.position;
        myCamera.transform.position = tarPos + new Vector3(0, 40, -10);
        myCamera.transform.LookAt(tarPos);

        GameLogin.instance.m_SelfSnake.Update();
    }
}
