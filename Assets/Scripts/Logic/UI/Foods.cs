using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Snake3D;

public class Foods : MonoBehaviour
{
    public Dictionary<UInt32, GameObject> mFoodList;
    void Start()
    {
        if (mFoodList == null)
        {
            mFoodList = new Dictionary<UInt32, GameObject>();
        }
    }

    void OnEnable()
    {
        Messager.Instance.AddNotification("AddFoods", funtionAdd);
        Messager.Instance.AddNotification("DeleteFoods", funtionDel);
    }

    void OnDestroy()
    {
        Messager.Instance.RemoveNotification("AddFoods",funtionAdd);
        Messager.Instance.RemoveNotification("DeleteFoods", funtionDel);
    }


    private void funtionAdd(Notification msg)
    {
       
        List<FoodItem> _foodList = msg["Addfoods"] as List<FoodItem>;
        for (int i = 0; i < _foodList.Count; i++)
        {
            FoodItem item = _foodList[i];
            Vector3 _postion = new Vector3(item.GetPosX(), 0, item.GetPosY());
            string _path = ResConfig.THEME_PATH + UserLogic.Instance.ThemeUsing + "/body";
            GameObject _bodyRes = Resources.Load<GameObject>(_path);
            GameObject _food = GameObject.Instantiate(_bodyRes, _postion, Quaternion.identity) as GameObject;
            _food.name = "Food";
            _food.transform.parent = transform.GetChild(0);
            mFoodList.Add(item.GetId(), _food);
        }
       
    }

    private void funtionDel(Notification msg)
    {
        UInt32 deleteKey = (UInt32) msg["Deletefoods"];
        if (mFoodList.ContainsKey(deleteKey))
        {
            mFoodList.Remove(deleteKey);
            Destroy(mFoodList[deleteKey]);
        }
       
    }
    
}
