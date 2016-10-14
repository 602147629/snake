using System;
using System.Collections.Generic;

public class NetIDContainer
{
    private static Dictionary<Int16, string> id_name = new Dictionary<Int16, string>();
    private static Dictionary<string, Int16> name_id = new Dictionary<string, Int16>();

    static NetIDContainer()
    {
        AddIdName(0, "MsgMsgInit");
        AddNameId("MsgMsgInit", 0);
    }
    public static Int16 GetMessageId(string key)
    {
        return name_id[key];
    }

    public static string GetMessageKey(Int16 id)
    {
        return id_name[id];
    }

    public static void AddIdName(Int16 id,string key)
    {
        if(!id_name.ContainsKey(id))
            id_name.Add(id, key);
    }
    public static void AddNameId(string key, Int16 id)
    {
        if (!name_id.ContainsKey(key))
            name_id.Add(key, id);
    }
}
