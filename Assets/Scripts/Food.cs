using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	   

	}

    public void FoodAppear()
    {
        string path = ResConfig.THEME_PATH + UserLogic.Instance.ThemeUsing + "/body";
        GameObject bodyRes = Resources.Load<GameObject>(path);
        int postionX = Random.Range(0, 30);
        int postionY = Random.Range(0, 40);
        Vector3 postion = new Vector3(postionX, 0, postionY);
        /*
         *  @function 在指定位置创建食物
         *  @params1：物体的预制
         *  @params2：postion
         *  @params3:旋转方式
         * 
         */
        GameObject.Instantiate(bodyRes.transform.GetChild(0).gameObject, postion,Quaternion.identity);
    }
}
