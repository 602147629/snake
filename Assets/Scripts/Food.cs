using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{
    public static Food Instance = new Food();
    private int i = 0;
    private Food() { }
    public Dictionary<String, GameObject> mFoodMap;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void FoodAppear()
    {
        string path = ResConfig.THEME_PATH + UserLogic.Instance.ThemeUsing + "/body";
        GameObject bodyRes = Resources.Load<GameObject>(path);
        int postionX = Random.Range(0, 30);
        int postionY = Random.Range(0, 40);
        Vector3 postion = new Vector3(postionX, 0, postionY);
        Debug.Log(postion);
        i++;
        GameObject _food = GameObject.Instantiate(bodyRes.transform.GetChild(0).gameObject,postion,Quaternion.identity) as GameObject;
        _food.name = "Food"+i;
        if (mFoodMap == null)
        {
            mFoodMap = new Dictionary<string, GameObject>();
        }
        mFoodMap.Add(i.ToString(), _food);
        Debug.Log("***************"+mFoodMap.Count);
    }

    public void FoodDisapper(String key)
    {
        if (mFoodMap.ContainsKey(key))
        {
            GameObject mFood = mFoodMap[key];
            Destroy(mFood);
        }
    }
}
