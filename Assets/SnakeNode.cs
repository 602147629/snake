using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeNode {
    public int _surplusLength;
    public float _speed;
    public string _name;
    public int _jg = 7;
    public List<GameObject> _nodeList = new List<GameObject>();
    public List<Vector3> _targetPos = new List<Vector3>();
    // Use this for initialization
    public void Init(GameObject snakeModel,string name, Vector3 pos, int surplusLength, float speed)
    {
        _name = name;
        _surplusLength = surplusLength;
        _speed = speed;


        GameObject temp = new GameObject();
        temp.name = name;
        for (int i = 0; i < _surplusLength; i++)
        {
            GameObject node = GameObject.Instantiate(snakeModel);

            node.name = i.ToString();
            node.transform.parent = temp.transform;
            node.transform.position = pos;
            node.transform.localScale = Vector3.one;
            node.SetActive(true);
            _nodeList.Add(node);
        }
        _targetPos.Add(pos);
    }
    public void Move(Vector3 tarPos, float deltaTime)
    {
        _targetPos.Insert(0, _targetPos[0] + tarPos.normalized * _speed * deltaTime);
    }
    public void Update()
    {
        for (int i = 0; i < _surplusLength; i++)
        {
            Vector3 tempV = _targetPos[i * _jg < _targetPos.Count ? i * _jg : _targetPos.Count - 1];
            _nodeList[i].transform.LookAt(tempV);
            _nodeList[i].transform.position = tempV;
        }
    }
}
