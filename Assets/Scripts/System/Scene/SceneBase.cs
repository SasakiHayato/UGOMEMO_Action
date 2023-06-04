using UnityEngine;

/// <summary>
/// シーンの立ち上げ、破棄時にイベントを追加するためのインターフェース
/// </summary>
public interface ILoader
{
    /// <summary>
    /// シーン立ち上げ時の処理
    /// </summary>
    System.Collections.IEnumerator OnLoad();

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
    // protected method
    //=================================================

    /// <summary>
    /// シーン立ち上げ時の処理.
    /// UnityEngine.Start()
    /// </summary>
    protected abstract System.Collections.IEnumerator Setup();

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

    System.Collections.IEnumerator ILoader.OnLoad()
    {
        yield return Setup();
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
        var game_local_data = new GameLocalData() as ILoader;
        var sound_manager = new SoundManager() as ILoader;
        var input_manager = new InputManager() as ILoader;
        var scene = this as ILoader;

        StartCoroutine(game_local_data.OnLoad());
        StartCoroutine(sound_manager.OnLoad());
        StartCoroutine(input_manager.OnLoad());
        StartCoroutine(scene.OnLoad());
    }

    private void Update()
    {
        UpdateEvent();
    }
}
