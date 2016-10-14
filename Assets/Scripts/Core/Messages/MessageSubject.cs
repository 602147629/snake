using UnityEngine;
using System.Collections;

public class MessageSubject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if(GUI.Button(new Rect(10,10,100,40),"Click Me"))
        {
            Notification notify = new Notification(NotificationNames.NOTIFY_TEST);
            notify["age"] = 10;
            notify.Send();
        }
    }
        
}
