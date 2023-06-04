using System.Collections.Generic;
using UnityEngine;
using PoolSystem;

/// <summary>
/// サウンド管理クラス
/// </summary>
public class SoundManager : SingletonBehaviour<SoundManager>
{
    //==============================================================
    // variable
    //==============================================================

    private SoundPlayer _bgmSounder;
    private Pool<SoundPlayer> _soundPool;
    private IReadOnlyList<SoundDataAsset> _soundDataAssets;

    //==============================================================
    // public method
    //==============================================================

    /// <summary>
    /// サウンドの再生
    /// </summary>
    /// <param name="sound_type"></param>
    /// <param name="address"></param>
    public void Play(Define.Sound.Type sound_type, SoundAddress address)
    {
        SoundDataAsset.IReadOnlySoundData sound_data = null;

        for (int index = 0; index < _soundDataAssets.Count; index++)
        {
            var data = _soundDataAssets[index];
            if (data.SoundType == sound_type)
            {
                sound_data = data.GetData(address);
                break;
            }
        }

        if (sound_data == null)
        {
            Debug.LogWarning("一致するサウンドデータが見つかりませんでした。処理をスキップします。");
            return;
        }

        var pool = _soundPool.Get();

        if (sound_type == Define.Sound.Type.BGM)
        {
            if (_bgmSounder != null) _bgmSounder.Stop();
            _bgmSounder = pool.Object;
        }

        pool.Object.SetData(sound_data, sound_type);
        _soundPool.Set(pool.ID);
    }

    /// <summary>
    /// 現在BGMの停止
    /// </summary>
    public void StopBGM()
    {
        _bgmSounder.Stop();
        _bgmSounder = null;
    }

    public void SetController(SoundController controller)
    {
        _soundDataAssets = controller.SoundDataAsset;
        _soundPool = new Pool<SoundPlayer>(controller.Sounder);
    }

    //==============================================================
    // protected method
    //==============================================================

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Initialize()
    {
        
    }
}
