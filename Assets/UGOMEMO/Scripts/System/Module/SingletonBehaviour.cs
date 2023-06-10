using System.Collections;
/// <summary>
/// �P���Class��ۏ؂��邽�߂̃N���X
/// </summary>
/// <typeparam name="Class">�ΏۃN���X</typeparam>
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
}
