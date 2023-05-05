using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �T�E���h�f�[�^�p�A�Z�b�g�N���X
/// </summary>
public class SoundDataAsset : ScriptableObject
{
    //=======================================================
    // public interface
    //=======================================================

    /// <summary>
    /// �T�E���h�f�[�^��ǂݎ��p�ɂ��ĊO���֌��J���邽�߂̃C���^�[�t�F�[�X
    /// </summary>
    public interface IReadOnlySoundData
    {
        /// <summary>
        /// �T�E���h�N���b�v
        /// </summary>
        AudioClip Clip { get; }

        /// <summary>
        /// �f�[�^�A�h���X
        /// </summary>
        SoundAddress Address { get; }

        /// <summary>
        /// ����
        /// </summary>
        float Volume { get; }

        /// <summary>
        /// �s�b�`
        /// </summary>
        float Pitch { get; }
    }

    //=======================================================
    // private class
    //=======================================================

    /// <summary>
    /// �T�E���h�̃f�[�^�N���X
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
        /// �f�[�^�����p�̏�����
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
    /// �f�[�^�쐬���̏���.
    /// Editor��ō쐬�����f�[�^����ꍞ�ނ��߂Ȃ̂ŁA���ł͌Ă΂Ȃ�.
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
    /// �T�E���h�^�C�v�̎w��.
    /// Editor��ō쐬�����f�[�^����ꍞ�ނ��߂Ȃ̂ŁA���ł͌Ă΂Ȃ�.
    /// </summary>
    /// <param name="sound_type"></param>
    public void SetSoundType(Define.Sound.Type sound_type)
    {
        _soundType = sound_type;
    }

    #endif
}
