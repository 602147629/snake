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

    protected override void OnLoad()
    {
        selfRectTrans = transform.GetChild(0).GetComponent<RectTransform>();
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
            //计算 蛇上次行走的方向跟本次摇杆方向的夹角
            float angele = Vector2.Angle(toPoint, point);

            if (angele < 20)
            {
                toPoint = point;
                GameLogin.instance.SetSelfTo(new Vector3(toPoint.x, 0, toPoint.y));
            }
            //角度大于20 让蛇上次行走的方向 渐变到本次摇杆方向的夹角 每次加10
            else if (Vector3.Cross(new Vector3(toPoint.x, 0, toPoint.y), new Vector3(point.x, 0, point.y)).y > 0)
            {
                toPoint = RotationMatrix(toPoint, 10);
               // Debug.Log("旋转之前的向量为-------" + toPoint);
            }

            else
            {

                toPoint = RotationMatrix(toPoint, -10);
            }


            GameLogin.instance.SetSelfTo(new Vector3(toPoint.x, 0, toPoint.y));


        }

        if (

        Input.GetMouseButtonUp(0))
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

