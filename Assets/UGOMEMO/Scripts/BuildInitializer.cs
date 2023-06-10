using UnityEngine;

/// <summary>
/// �Q�[�������グ���
/// </summary>
public static class BuildInitializer
{
    /// <summary>
    /// �����グ�I������
    /// [TRUE:����][FALSE:������]
    /// </summary>
    public static bool IsBuild { get; private set; }

    /// <summary>
    /// ������
    /// �V�[�����[�h�O
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Build()
    {
        InitializeBehaviour();
        IsBuild = true;
    }

    /// <summary>
    /// SingletonBehaviour ���p�����Ă���N���X�͂����ŏ�����
    /// </summary>
    private static void InitializeBehaviour()
    {
        new GameLocalData();
        new SoundManager();
        new InputManager();
        new AchivementManager();
    }
}
