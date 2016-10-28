using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Snake3D;
using UnityEditor.VersionControl;
using Random = UnityEngine.Random;
[Module("FoodMoudle", true)]
public class FoodMoudle : ModuleBase
{
    private int i = 0;
    public Dictionary<Int32, FoodItem> mFoodList;
    // Use this for initialization
    public override void OnLoad()
    {
        NetManager.Instance.AddNetCallback("MsgAddFood", FoodNetInit);
        NetManager.Instance.AddNetCallback("MsgDelFood", FoodNetDel);
        if (mFoodList == null) ;
        {
            mFoodList = new Dictionary<Int32, FoodItem>();
        }
    }

    public override void OnRelease()
    {
        NetManager.Instance.RemoveNetCallback("MsgAddFood", FoodNetInit);
        NetManager.Instance.RemoveNetCallback("MsgDelFood", FoodNetDel);
    }

    public void FoodNetInit(object msg)
    {
        Debug.Log("+++++++链接成功28--++++++");
        MsgAddFood initMsg = msg as MsgAddFood;
        List<MsgFoodStruct> msgConfs = initMsg.FoodList;
        for (int i = 0; i < msgConfs.Count; i++)
        {
            MsgFoodStruct item = msgConfs[i];
            FoodItem items = new FoodItem();
            items.SetId((Int32)item.Id);
            items.SetPosX((float)item.PosX);
            items.SetPosY((float)item.PosY);
            items.SetRadius((float)item.Radius);
            items.SetScore((Int32)item.Score);
            mFoodList.Add(items.GetId(), items);
            Debug.Log("------mFoodList--++++++" + mFoodList.Count);
            notifyAddFood(items.GetId()+"",items);
        }
 
    }
 
    public void FoodNetDel(object msg)
    {
        Debug.Log("------链接成功--------");
        List<UInt32> msgConfs = (List<UInt32>)msg ;
        for (int j = 0; j < msgConfs.Count; j++)
        {
            Int32 _deletekey = (Int32) msgConfs[i];

            if (mFoodList.ContainsKey(_deletekey))
            {
                mFoodList.Remove(_deletekey);
                notifyDeleteFood(_deletekey);
            }
        }

      

    }

    public void notifyAddFood(String key, FoodItem item)
    {
        Notification notify = new Notification("AddFoods", null);
        notify["Addfoods"] = item;
        notify["AddFoodsKey"] = key;
        notify.Send();
    }
    public void notifyDeleteFood(Int32 key)
    {
        Notification notify = new Notification("DeleteFoods", null);
        notify["Deletfoods"] = key;
        notify.Send();
    }
}
