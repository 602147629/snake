using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snake
{
    public int _surplusLength;
    public float _speed;
    public string _name;
    public int _jg = 4;
    private GameObject _parent;
    public List<GameObject> _nodeList = new List<GameObject>();
    public List<Vector3> _targetPos = new List<Vector3>();

    // Use this for initialization
    public void Init(string name, Vector3 pos, int surplusLength, float speed)
    {
        _name = name;
        _speed = speed;

        _parent = new GameObject();
        _parent.name = name;
        _targetPos.Add(pos);
        CreateHead();
        SetLength(surplusLength);
    }
    public void Move(Vector3 tarPos, float deltaTime)
    {
        _targetPos.Insert(0, _targetPos[0] + tarPos.normalized * _speed * deltaTime);
    }
    public void Update(float deltaTime)
    {
        for (int i = 0; i < _surplusLength; i++)
        {
            Vector3 tempV = _targetPos[i * _jg < _targetPos.Count ? i * _jg : _targetPos.Count - 1];
            _nodeList[i].transform.LookAt(tempV);
            //float angle = 
            //_nodeList[i].transform.rotation = Quaternion.Slerp()
            _nodeList[i].transform.position = Vector3.MoveTowards(_nodeList[i].transform.position, tempV, _speed * deltaTime);
        }
        if (_surplusLength > 0)
        {
            int a = (_surplusLength - 1) * _jg + 1;
            if (a < _nodeList.Count)
            {
                _nodeList.RemoveRange(a, _nodeList.Count - a);
            }
        }
    }
    public void SetLength(int length)
    {
        _surplusLength = length;
        string path = ResConfig.THEME_PATH + UserLogic.Instance.ThemeUsing + "/body";
        GameObject bodyRes = Resources.Load<GameObject>(path);
        for (int i = _nodeList.Count; i < _surplusLength; i++)
        {
            GameObject node = GameObject.Instantiate(bodyRes);
            node.name = i.ToString();
            node.transform.parent = _parent.transform;
            _nodeList.Add(node);
        }
        for (int i = _nodeList.Count - 1; i > _surplusLength - 1; i--)
        {
            GameObject.Destroy(_nodeList[i]);
            _nodeList.RemoveAt(i);
        }
    }

    private void CreateHead()
    {
        string path = ResConfig.THEME_PATH + UserLogic.Instance.ThemeUsing + "/head";
        GameObject modelRes = Resources.Load<GameObject>(path);
        if (null == modelRes)
        {
            return;
        }
        GameObject head = GameObject.Instantiate<GameObject>(modelRes);
        head.transform.SetParent(_parent.transform);
        _nodeList.Add(head);
    }
}
