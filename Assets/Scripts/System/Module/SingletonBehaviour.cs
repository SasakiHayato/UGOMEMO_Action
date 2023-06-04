/// <summary>
/// �P���Class��ۏ؂��邽�߂̃N���X
/// </summary>
/// <typeparam name="Class">�ΏۃN���X</typeparam>
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
    /// ������
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
    /// ����������
    /// </summary>
    protected abstract void Initialize();

    /// <summary>
    /// SceneLoad���̏���
    /// UnityEngine.Start()
    /// </summary>
    protected virtual System.Collections.IEnumerator OnLoad() { yield return null; }

    /// <summary>
    /// Scene�j�����̏���
    /// UnityEngine.OnDestroy()
    /// </summary>
    protected virtual void UnLoad() { }
    
    /// <summary>
    /// �C���X�^���X�j�����̏���
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
