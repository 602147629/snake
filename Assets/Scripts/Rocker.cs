using UnityEngine;
using System.Collections;

public class Rocker : MonoBehaviour {
    public GameObject obj;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            obj.transform.position = Input.mousePosition;
            if (Vector3.Distance(obj.transform.localPosition,Vector3.zero) > gameObject.GetComponent<RectTransform>().sizeDelta.x / 2)
            {
                obj.transform.localPosition = obj.transform.localPosition.normalized * gameObject.GetComponent<RectTransform>().sizeDelta.x / 2;
            }
            GameLogin.instance.SetSelfTo(new Vector3(obj.transform.localPosition.x,0, obj.transform.localPosition.y));
        }
        if (Input.GetMouseButtonUp(0))
        {
            obj.transform.localPosition = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameLogin.instance.SetSelfLength(GameLogin.instance.m_SelfSnake._surplusLength + 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameLogin.instance.SetSelfLength(GameLogin.instance.m_SelfSnake._surplusLength - 1);
        }
    }
}
