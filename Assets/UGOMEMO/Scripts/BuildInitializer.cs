using UnityEngine;

/// <summary>
/// ゲーム立ち上げ基盤
/// </summary>
public static class BuildInitializer
{
    /// <summary>
    /// 立ち上げ終了判定
    /// [TRUE:完了][FALSE:準備中]
    /// </summary>
    public static bool IsBuild { get; private set; }

    /// <summary>
    /// 初期化
    /// シーンロード前
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Build()
    {
        InitializeBehaviour();
        IsBuild = true;
    }

    /// <summary>
    /// SingletonBehaviour を継承しているクラスはここで初期化
    /// </summary>
    private static void InitializeBehaviour()
    {
        new GameLocalData();
        new SoundManager();
        new InputManager();
        new AchivementManager();
    }
}
