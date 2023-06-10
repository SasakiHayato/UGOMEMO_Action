/// <summary>
/// サウンドの音量データクラス
/// </summary>
public static class SoundVolumeData
{
    private static float MAX_VOLUME = 1;

    //================================================
    // variable
    //================================================

    static float _masterVolume = 0.5f;
    static float _bgmVolume = 0.5f;
    static float _seVolume = 0.5f;
    static float _environmentVoliume = 0.5f;

    //================================================
    // property
    //================================================

    /// <summary>
    /// マスター音量
    /// </summary>
    public static float MasterVolume 
    {
        get => _masterVolume;
        set => _masterVolume = value > MAX_VOLUME ? MAX_VOLUME : value;
    }

    /// <summary>
    /// BGM音量
    /// </summary>
    public static float BGMVolume
    {
        get => _bgmVolume;
        set => _bgmVolume = value > MAX_VOLUME ? MAX_VOLUME : value;
    }

    /// <summary>
    /// SE音量
    /// </summary>
    public static float SEVolume
    {
        get => _seVolume;
        set => _seVolume = value > MAX_VOLUME ? MAX_VOLUME : value;
    }

    /// <summary>
    /// 環境音
    /// </summary>
    public static float EnvironmentVolume
    {
        get => _environmentVoliume;
        set => _environmentVoliume = value >= MAX_VOLUME ? MAX_VOLUME : value;
    }
}
