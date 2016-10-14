using System.Collections.Generic;

/// <summary>
/// 消息的触发回调函数
/// </summary>
/// <param name="notify"></param>
public delegate void NotificationHandler(Notification notify);
/// <summary>
/// 消息
/// </summary>
public class Messager
{
    public static readonly Messager Instance = new Messager();

    private Dictionary<string, List<NotificationHandler>> m_MsgMap = new Dictionary<string, List<NotificationHandler>>();

    private Messager() { }

    public void AddNotification(string notificationName, NotificationHandler callFun)
    {
        if (!m_MsgMap.ContainsKey(notificationName))
        {
            List<NotificationHandler> callList = new List<NotificationHandler>();
            callList.Add(callFun);
            m_MsgMap.Add(notificationName, callList);
        }
        else
        {
            m_MsgMap[notificationName].Add(callFun);
        }
    }

    public void RemoveNotification(string notificationName, NotificationHandler callFun)
    {
        if (m_MsgMap.ContainsKey(notificationName))
        {
            m_MsgMap[notificationName].Remove(callFun);
        }
    }

    public bool HasNotification(string notificationName,NotificationHandler callFun)
    {
        return m_MsgMap.ContainsKey(notificationName) ? m_MsgMap[notificationName].Contains(callFun) : false;
    }

    public void Excute(string notificationName,Notification notify)
    {
        if (m_MsgMap.ContainsKey(notificationName))
        {
            for(int i = 0,size = m_MsgMap[notificationName].Count; i < size; i++)
            {
                m_MsgMap[notificationName][i](notify);
            }
        }
    }

}
