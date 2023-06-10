using System.Collections;
/// <summary>
/// 単一のClassを保証するためのクラス
/// </summary>
/// <typeparam name="Class">対象クラス</typeparam>
public abstract class SingletonBehaviour<Class> where Class : SingletonBehaviour<Class>
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
}
