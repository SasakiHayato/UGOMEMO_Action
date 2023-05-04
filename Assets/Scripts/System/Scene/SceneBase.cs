using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーンの立ち上げ、破棄時にイベントを追加するためのインターフェース
/// </summary>
public interface ILoader
{
    /// <summary>
    /// シーン立ち上げ時の処理
    /// </summary>
    void OnLoad();

    /// <summary>
    /// シーン破棄時の処理
    /// </summary>
    void UnLoad();
}


/// <summary>
/// 各Scene管理の基底クラス
/// </summary>
public abstract class SceneBase : MonoBehaviour, ILoader
{
    //=================================================
    // variable
    //=================================================

    private List<ILoader> _loadList;

    //=================================================
    // protected method
    //=================================================

    /// <summary>
    /// シーン立ち上げ時の処理.
    /// UnityEngine.Start()
    /// </summary>
    protected abstract void Setup();

    /// <summary>
    /// マイフレームの処理.
    /// UnityEngine.Update()
    /// </summary>
    protected virtual void UpdateEvent() { }

    /// <summary>
    /// シーン破棄時の処理.
    /// UnityEngine.OnDestroy()
    /// </summary>
    protected virtual void OnDestroyEvent() { }

    //=================================================
    // ILoader interface
    //=================================================

    void ILoader.OnLoad()
    {
        Setup();
    }

    void ILoader.UnLoad()
    {
        OnDestroyEvent();
    }

    //=================================================
    // Unity method
    //=================================================

    private void Awake()
    {
        _loadList = new List<ILoader>();

        new GameLocalData();
        _loadList.Add(new InputManager());
        _loadList.Add(this);
    }

    private void Start()
    {
        _loadList.ForEach(l => l.OnLoad());
    }

    private void Update()
    {
        UpdateEvent();
    }

    private void OnDestroy()
    {
        _loadList.ForEach(l => l.UnLoad());
    }
}
