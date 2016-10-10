using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogin : MonoBehaviour
{
    public Camera myCamera;
    public GameObject m_SnakeObjModel;
    public List<SnakeNode> m_OtherSnakeObj;
    public SnakeNode m_SelfSnake;

    private float time;
    private Vector3 to;
    // Use this for initialization
    void Start()
    {
        m_SelfSnake = CreateSnake("asdfasdfas");
        to = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tarPos = m_SelfSnake._nodeList[0].transform.position;
        myCamera.transform.position = tarPos + new Vector3(0, 40, -10);
        myCamera.transform.LookAt(tarPos);

        if (time < 1f)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
            to = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        }
        m_SelfSnake.Move(to, Time.deltaTime);
        m_SelfSnake.Update();
    }
    public SnakeNode CreateSnake(string name)
    {
        SnakeNode snake = new SnakeNode();
        snake.Init(m_SnakeObjModel, name, Vector3.zero, 20, 10f);
        return snake;
    }
}
