using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RoomSelectedUI : UIBase
{
    public Button btnThemeRoom;
    public Button btnFreeRoom;
    protected override void OnLoad()
    {
        UIManager.SetButtonClick(btnThemeRoom.gameObject, OnEnterRoom, 0, 0);
        UIManager.SetButtonClick(btnFreeRoom.gameObject, OnEnterRoom, 1, 0);
        base.OnLoad();
    }

    void OnEnterRoom(GameObject obj, int param1, int param2)
    {
        if(0 == param1)
        {
            Game.Instance().CreateScene("Game",typeof(GameScene));
        }
        else
        {

        }
    }
}
