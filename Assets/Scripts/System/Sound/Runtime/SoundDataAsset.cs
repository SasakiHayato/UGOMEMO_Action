using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サウンドデータ用アセットクラス
/// </summary>
public class SoundDataAsset : ScriptableObject
{
    //=======================================================
    // public interface
    //=======================================================

    /// <summary>
    /// サウンドデータを読み取り用にして外部へ公開するためのインターフェース
    /// </summary>
    public interface IReadOnlySoundData
    {
        /// <summary>
        /// サウンドクリップ
        /// </summary>
        AudioClip Clip { get; }

        /// <summary>
        /// データアドレス
        /// </summary>
        SoundAddress Address { get; }

        /// <summary>
        /// 音量
        /// </summary>
        float Volume { get; }

        /// <summary>
        /// ピッチ
        /// </summary>
        float Pitch { get; }
    }

    //=======================================================
    // private class
    //=======================================================

    /// <summary>
    /// サウンドのデータクラス
    /// </summary>
    [System.Serializable]
    private class Data : IReadOnlySoundData
    {
        //=======================================================
        // variable
        //=======================================================

        [SerializeField] AudioClip _audioClip;
        [SerializeField] SoundAddress _address;
        [SerializeField, Range(0, 1)] private float _volume;
        [SerializeField, Range(-3, 3)] private float _pitch;

        //=======================================================
        // constructor
        //=======================================================

        #if UNITY_EDITOR

        /// <summary>
        /// データ生成用の初期化
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="address"></param>
        public Data(AudioClip clip, SoundAddress address)
        {
            _audioClip = clip;
            _address = address;
        }

        #endif

        //=======================================================
        // IReadOnlySoundData interface
        //=======================================================

        AudioClip IReadOnlySoundData.Clip => _audioClip;
        SoundAddress IReadOnlySoundData.Address => _address;
        float IReadOnlySoundData.Volume => _volume;
        float IReadOnlySoundData.Pitch => _pitch;
    }

    [SerializeField, ReadOnly] Define.Sound.Type _soundType;
    [SerializeField] List<Data> _soundDataList;

    //=======================================================
    // editor method
    //=======================================================

    #if UNITY_EDITOR

    /// <summary>
    /// データ作成時の処理.
    /// Editor上で作成したデータを入れ込むためなので、他では呼ばない.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="address"></param>
    public void CreateData(AudioClip clip, SoundAddress address)
    {
        if (_soundDataList == null)
        {
            _soundDataList = new List<Data>();
        }

        _soundDataList.Add(new Data(clip, address));
    }

    /// <summary>
    /// サウンドタイプの指定.
    /// Editor上で作成したデータを入れ込むためなので、他では呼ばない.
    /// </summary>
    /// <param name="sound_type"></param>
    public void SetSoundType(Define.Sound.Type sound_type)
    {
        _soundType = sound_type;
    }

    #endif
}
