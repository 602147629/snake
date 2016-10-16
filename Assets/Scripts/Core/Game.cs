using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using FSM;

public class Game : MonoBehaviour {
    private SceneBase curScene ;
    public SceneBase CurScene{get { return curScene; }}
    public Camera camera2D { get; private set; }

    private Transform container3D;
    public Transform Container3D
    {
        get { return container3D; }
    }

    void Start () {
        Singleton.GetInstance("TimeUtil");
        ModuleManager.Instance.Init();
        InitUIRoot();
	}
	
	void Update ()
    {
        if (null != curScene) curScene.OnUpdate(Time.deltaTime);
	}

    public static Game Instance()
    {
        return Singleton.GetInstance("Game") as Game;
    }

    public void BeginCoroutine(Func<IEnumerator> func)
    {
        StartCoroutine(func());
    }

    public void SetContainer3D(Transform container)
    {
        container3D = container;
    }

    private void InitUIRoot()
    {
        GameObject ui_Root = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/Canvas"));
        uiRoot = ui_Root;
        UIManager.Instance.InitUIManager(uiRoot.transform.FindChild("UI"));
        DontDestroyOnLoad(uiRoot);
        UIManager.Instance.OpenUI(UINames.Snake_Room_SelectUI);
    }

    public void UnRegisterUpdateObj<T>() where T : IUpdate
    {

    }

    public void CreateScene(string sceneName,Type sceneType)
    {
        toLoadSceneData = new SceneData(sceneName, sceneType) ;
        BeginCoroutine(LoadScene);
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation opt = SceneManager.LoadSceneAsync(toLoadSceneData.sceneName);
        yield return opt;
        if(null != curScene) curScene.OnRelease();
        curScene = Activator.CreateInstance(toLoadSceneData.sceneType) as SceneBase;
        curScene.OnLoad();
    }

    private SceneData toLoadSceneData;
    private GameObject uiRoot;
}
