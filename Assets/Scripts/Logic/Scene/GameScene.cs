using UnityEngine;
using System.Collections;

public class GameScene : SceneBase {
    public override void OnLoad()
    {
        UIManager.Instance.CloseUI(UINames.Snake_Room_SelectUI);
        UIManager.Instance.OpenUI(UINames.RockerUI);
        base.OnLoad();
    }
}
