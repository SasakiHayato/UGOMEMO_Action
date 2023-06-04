/// <summary>
/// 単一のClassを保証するためのクラス
/// </summary>
/// <typeparam name="Class">対象クラス</typeparam>
public abstract class SingletonBehaviour<Class> : System.IDisposable where Class : SingletonBehaviour<Class>
{
    //================================================
    // variable
    //================================================

    private static Class s_class;

    //================================================
    // property
    //================================================

    public static Class Instance => s_class;

    //================================================
    // constructor
    //================================================

    /// <summary>
    /// 初期化
    /// </summary>
    public SingletonBehaviour()
    {
        if (s_class != null) return;
        s_class = (Class)this;

        Initialize();
    }

    //================================================
    // protected method
    //================================================

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected abstract void Initialize();

    /// <summary>
    /// SceneLoad時の処理
    /// UnityEngine.Start()
    /// </summary>
    protected virtual System.Collections.IEnumerator OnLoad() { yield return null; }

    /// <summary>
    /// Scene破棄時の処理
    /// UnityEngine.OnDestroy()
    /// </summary>
    protected virtual void UnLoad() { }
    
    /// <summary>
    /// インスタンス破棄時の処理
    /// </summary>
    protected virtual void Dispose() { }

    //================================================
    // System.IDsposable
    //================================================

    void System.IDisposable.Dispose()
    {
        s_class = null;
        System.GC.Collect();
        Dispose();
    }

    
}
