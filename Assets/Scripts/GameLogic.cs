using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct NodeData
{
    public string modelId;
    public Vector2 nodePos;
    public Color color;
}
public class GameLogic
{
    public readonly static GameLogic instance = new GameLogic();
    
    public List<Snake> m_OtherSnakeObj;
    public Snake m_SelfSnake;
    private Vector3 m_ToDirection;
    private GameView m_GameView;
    // Use this for initialization

    public void Init(GameView gv)
    {
        m_GameView = gv;
        m_ToDirection = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    public void Update(float deltaTime)
    {
        GameLogic.instance.m_SelfSnake.Move(m_ToDirection, deltaTime);
    }
    public Snake CreateSnake(string name)
    {
        Snake snake = new Snake();
        snake.Init(name, Vector3.zero, 20, 0.3f);
        return snake;
    }

    public Snake CreateSnake(List<NodeData> data)
    {
        Snake snake = new Snake();
        //snake.Init(name, Vector3.zero, 20, 0.3f);
        return snake;
    }
    public void SetSelfTo(Vector3 to)
    {
        m_ToDirection = to;
    }
    public void SetSelfLength(int length)
    {
        m_SelfSnake.SetLength(length);
    }

}


