using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{
    public static Food Instance = new Food();
    public GameObject obj;
    private int i = 0;
    private Food() { }
    public Dictionary<Int32, GameObject> mFoodMap;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    //public void initFoods(Dictionary<String,Object> foodFromNet)
    //{
    //    if (mFoodMap==null)
    //    {
    //        mFoodMap = new Dictionary<string, GameObject>();
    //    }
    //    foreach(KeyValuePair<string, Object> kvp in foodFromNet)
    //    {           
    //    }
    //}

    public void FoodAppear(FoodItem item)
    {
        string path = ResConfig.THEME_PATH + UserLogic.Instance.ThemeUsing + "/body";
        GameObject bodyRes = Resources.Load<GameObject>(path);
        //int postionX = Random.Range(0, 30);
        //int postionY = Random.Range(0, 40);
        Vector3 postion = new Vector3(item.GetPosX(), 0, item.GetPosY());
        GameObject _food = GameObject.Instantiate(bodyRes.transform.GetChild(0).gameObject,postion,Quaternion.identity) as GameObject;
        _food.name = "Food";
        _food.transform.parent = obj.transform;
        if (mFoodMap == null)
        {
            mFoodMap = new Dictionary<Int32, GameObject>();
        }
        mFoodMap.Add(item.GetId(), _food);
        Debug.Log("***************"+mFoodMap.Count);
    }

    public void FoodDisapper(Int32 key)
    {
        if (mFoodMap.ContainsKey(key))
        {
            GameObject mFood = mFoodMap[key];
            Destroy(mFood);
        }
    }
}
