using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rocker : UIBase
{
    public GameObject obj;
    public Vector3 oldV;
    public Vector2 toPoint = new Vector2(0, 1);
    RectTransform selfRectTrans;
    private List<Vector2> mList;
	private GameMudule gameMudule;
    protected override void OnLoad()
    {
        selfRectTrans = transform.GetChild(0).GetComponent<RectTransform>();
		gameMudule = ModuleManager.Instance.GetModule<GameMudule>();
        base.OnLoad();
    }

    protected override void OnUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 point = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(selfRectTrans, Input.mousePosition,
                UIManager.Instance.UICamera, out point);
            obj.transform.localPosition = new Vector3(point.x, point.y, 0);
            if (Vector3.Distance(obj.transform.localPosition, Vector3.zero) > selfRectTrans.sizeDelta.x / 2)
            {
                obj.transform.localPosition = obj.transform.localPosition.normalized * selfRectTrans.sizeDelta.x / 2;
            }
//			gameMudule.SetSelfTo(new Vector3(obj.transform.localPosition.x, 0,obj.transform.localPosition.y));
			gameMudule.MsgMove(obj.transform.localPosition.x,obj.transform.localPosition.y);
        }

        if (

        Input.GetMouseButtonUp(0))
        {
            obj.transform.localPosition = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            gameMudule.SetSelfLength(gameMudule.m_SelfSnake._surplusLength + 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            gameMudule.SetSelfLength(gameMudule.m_SelfSnake._surplusLength - 1);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
           // gameMudule.se();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
           // gameMudule.deleteFood("1");
        }
        base.OnUpdate();
    }

    /*
     根据原始向量值 和 旋转的角度 算出旋转后的向量值
     @params1 原始向量 
     @params2 旋转角度
     @result 旋转后角度
    */
    private Vector2 RotationMatrix(Vector2 v, float angle)
    {
        var x = v.x;
        var y = v.y;
        var sin = Math.Sin(Math.PI * angle / 180);
        var cos = Math.Cos(Math.PI * angle / 180);
        var newX = x * cos + y * sin;
        var newY = x * -sin + y * cos;
        return new Vector2((float)newX, (float)newY);
    }

}

