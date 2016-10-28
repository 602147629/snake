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
    public Dictionary<UInt32, FoodItem> mFoodList;
    // Use this for initialization
    public override void OnLoad()
    {
        NetManager.Instance.AddNetCallback("MsgAddFood", FoodNetInit);
        NetManager.Instance.AddNetCallback("MsgDelFood", FoodNetDel);
        if (mFoodList == null) ;
        {
            mFoodList = new Dictionary<UInt32, FoodItem>();
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
        List<FoodItem> itemList =new List<FoodItem>();
        for (int i = 0; i < msgConfs.Count; i++)
        {
            MsgFoodStruct item = msgConfs[i];
            FoodItem items = new FoodItem();
            items.SetId((UInt32)item.Id);
            items.SetPosX((float)item.PosX);
            items.SetPosY((float)item.PosY);
            items.SetRadius((float)item.Radius);
            items.SetScore((UInt32)item.Score);
            mFoodList.Add(items.GetId(), items);
            Debug.Log("------mFoodList--++++++" + mFoodList.Count);
            itemList.Add(items);
            //notifyAddFood(items.GetId()+"",items);

        }
        Notification notify = new Notification("AddFoods", null);
        notify["Addfoods"] = itemList;
        notify.Send();

    }
 
    public void FoodNetDel(object msg)
    {
        Debug.Log("------链接成功--------");
        List<UInt32> msgConfs = (List<UInt32>)msg ;
        for (int j = 0; j < msgConfs.Count; j++)
        {
            UInt32 _deletekey = (UInt32) msgConfs[i];

            if (mFoodList.ContainsKey(_deletekey))
            {
                mFoodList.Remove(_deletekey);
                notifyDeleteFood(_deletekey);
            }
        }

      

    }

    
        
    
    public void notifyDeleteFood(UInt32 key)
    {
        Notification notify = new Notification("DeleteFoods", null);
        notify["Deletfoods"] = key;
        notify.Send();
    }
}
