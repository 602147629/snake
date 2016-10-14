using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageObserver : MonoBehaviour {

    void OnEnable()
    {
        Messager.Instance.AddNotification(NotificationNames.NOTIFY_TEST, OnNotify);
    }

    void OnDisable()
    {
        Messager.Instance.RemoveNotification(NotificationNames.NOTIFY_TEST, OnNotify);
    }

    void OnNotify(Notification notify)
    {
        Debug.Log("OnNotify: " + notify["age"]);
        GetComponent<Text>().text = notify["age"].ToString();
    }
}
