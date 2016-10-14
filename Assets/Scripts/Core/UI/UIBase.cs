using UnityEngine;
using System.Collections;
using System;

public enum UIState
{
    Open,
    Closed
}

public abstract class UIBase : MonoBehaviour
{
    public UIState state { get; private set; }
    public UINames selfName { get;  set; }

    private float destroyTime = 10.0f;

	private void OnEnable ()
    {
        OnLoad();
	}

    private void Start()
    {
        OnOpenTween();
    }
	
	private void Update ()
    {
        OnUpdate();
	}

    private void OnDestroy()
    {
    }

    private void OnDisable()
    {
        OnRelease();
    }

    public virtual void OnLoadData(params object[] allParams) { }
    protected virtual void OnLoad() { }
    protected virtual void OnRelease() { }
    protected virtual void OnUpdate() { }
    /// <summary>
    /// 需要给一个默认的UI打开动画，没实现
    /// </summary>
    protected virtual void OnOpenTween() { }
}
