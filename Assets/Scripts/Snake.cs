using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snake
{
    public int _surplusLength;
    public float _speed;
    public string _name;
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
        _targetPos.Insert(0, _targetPos[0] + tarPos.normalized * _speed);
    }
    public void Update()
    {
        if (_targetPos.Count == 0)
        {
            return;
        }
        int idx = 0;
        Vector3 tempV = _targetPos[idx];
        _nodeList[0].transform.LookAt(tempV);
        _nodeList[0].transform.position = tempV;
        for (int i = 1; i < _surplusLength; i++)
        {
            bool isHavePos = false;
            float dis = 0;
            float _jg = (_nodeList[i - 1].transform.localScale.z + _nodeList[i].transform.localScale.z) * 0.48f;
            for (; idx < _targetPos.Count; idx++)
            {
                float tempDis = dis;
                dis += Vector3.Distance(tempV, _targetPos[idx]);
                if (dis > _jg)
                {
                    tempV = tempV + (_targetPos[idx] - tempV).normalized * (_jg - tempDis);
                    isHavePos = true;
                    break;
                }
                else
                {
                    tempV = _targetPos[idx];
                }
            }
            if (!isHavePos)
            {
                tempV = _targetPos[_targetPos.Count - 1];
            }
            _nodeList[i].transform.LookAt(tempV);
            _nodeList[i].transform.position = tempV;
        }
        if (_targetPos.Count > idx)
        {
            _targetPos.RemoveRange(idx, _targetPos.Count - idx);
        }
    }
    public void SetLength(int length)
    {
        _surplusLength = length;
        string path = ResConfig.THEME_PATH + UserLogic.Instance.ThemeUsing + "/body";
        GameObject bodyRes = Resources.Load<GameObject>(path);
        int bodyCount = bodyRes.transform.childCount;
        for (int i = _nodeList.Count; i < _surplusLength; i++)
        {
            int idx = (i - 1) % bodyCount;
            GameObject node = GameObject.Instantiate(bodyRes.transform.GetChild(idx).gameObject);
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
       // head.transform.localScale = Vector3.one;
        _nodeList.Add(head);
    }
}
