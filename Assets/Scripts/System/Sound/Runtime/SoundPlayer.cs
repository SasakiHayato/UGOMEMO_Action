using UnityEngine;
using PoolSystem;
using PoolSystem.Event;

/// <summary>
/// 音を鳴らすクラス
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour, IPool
{
    //============================================================
    // variable
    //============================================================

    [SerializeField] 
    private AudioSource _audioSource;

    private bool _canUpdate;
    private int _id;
    private Define.Sound.Type _soundType;
    private IPoolReleaseCallback _callback;

    private SoundDataAsset.IReadOnlySoundData _soundData;

    //============================================================
    // public method
    //============================================================

    /// <summary>
    /// サウンドデータのセット
    /// </summary>
    /// <param name="sound_data"></param>
    /// <param name="sound_type"></param>
    public void SetData(SoundDataAsset.IReadOnlySoundData sound_data, Define.Sound.Type sound_type)
    {
        _soundData = sound_data;
        _soundType = sound_type;
    }

    /// <summary>
    /// サウンドの停止
    /// </summary>
    public void Stop()
    {
        _audioSource.Stop();
    }

    //============================================================
    // IPool interface method
    //============================================================

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="release_callback"></param>
    /// <param name="pool_id"></param>
    void IPool.Initialize(IPoolReleaseCallback release_callback, int pool_id)
    {
        _callback = release_callback;
        _id = pool_id;

        _audioSource.playOnAwake = false;
    }

    /// <summary>
    /// 使用時の処理
    /// </summary>
    void IPool.Entry()
    {
        _audioSource.clip = _soundData.Clip;
        _audioSource.volume = _soundData.Volume;
        _audioSource.pitch = _soundData.Pitch;
        _audioSource.loop = _soundType == Define.Sound.Type.BGM;
        _audioSource.Play();
        _canUpdate = true;
    }

    /// <summary>
    /// 破棄時の処理
    /// </summary>
    void IPool.Release()
    {
        _canUpdate = false;
        _audioSource.clip = null;
    }

    //============================================================
    // unity method
    //============================================================

    private void Update()
    {
        if (!_canUpdate) return;
        if (!_audioSource.isPlaying) _callback.ReleaseCallback(_id);
    }
}
