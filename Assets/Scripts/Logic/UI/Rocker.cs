using UnityEngine;
using System.Collections;

public class Rocker : UIBase {
    public GameObject obj;

    RectTransform selfRectTrans;

    protected override void OnLoad()
    {
        selfRectTrans = transform.GetChild(0).GetComponent<RectTransform>();
        base.OnLoad();
    }
    Vector2 oldPoint = new Vector2(0,0);
    Vector2 toPoint = Vector2.zero;
    protected override void OnUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 point = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(selfRectTrans, Input.mousePosition, UIManager.Instance.UICamera, out point);
            if (Vector2.Angle(oldPoint,point) < 5.0f)
            {
                return;
            }
            toPoint = point;
            obj.transform.localPosition = new Vector3(point.x, point.y, 0);
            if (Vector3.Distance(obj.transform.localPosition, Vector3.zero) > selfRectTrans.sizeDelta.x / 2)
            {
                obj.transform.localPosition = obj.transform.localPosition.normalized * selfRectTrans.sizeDelta.x / 2;
            }
           
        }
        AngleTo();
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
        base.OnUpdate();
    }
    void AngleTo()
    {
        oldPoint = Vector2.Lerp(oldPoint, toPoint, 1.5f * Time.deltaTime);
        GameLogin.instance.SetSelfTo(new Vector3(oldPoint.x,0, oldPoint.y));
    }
}
