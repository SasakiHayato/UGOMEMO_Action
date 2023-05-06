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

        [SerializeField, ReadOnly] AudioClip _audioClip;
        [SerializeField, ReadOnly] SoundAddress _address;
        [SerializeField, ReadOnly, Range(0, 1)] private float _volume;
        [SerializeField, ReadOnly, Range(-3, 3)] private float _pitch;

        //=======================================================
        // constructor
        //=======================================================

        #if UNITY_EDITOR

        public AudioClip SetClip { set => _audioClip = value; }
        public SoundAddress SetAddress { set => _address = value; }

        ///// <summary>
        ///// �f�[�^�����p�̏�����
        ///// </summary>
        ///// <param name="clip"></param>
        ///// <param name="address"></param>
        //public Data(AudioClip clip, SoundAddress address)
        //{
        //    _audioClip = clip;
        //    _address = address;
        //}

        #endif

        //=======================================================
        // IReadOnlySoundData interface
        //=======================================================

        AudioClip IReadOnlySoundData.Clip => _audioClip;
        SoundAddress IReadOnlySoundData.Address => _address;
        float IReadOnlySoundData.Volume => _volume;
        float IReadOnlySoundData.Pitch => _pitch;
    }

    [SerializeField, ReadOnly] string _guid;
    [SerializeField, ReadOnly] Define.Sound.Type _soundType;
    [SerializeField] List<Data> _soundDataList;

    public Define.Sound.Type SoundType => _soundType;
    public IReadOnlySoundData GetData(SoundAddress address)
    {
        for (int index = 0; index < _soundDataList.Count; index++)
        {
            var data = _soundDataList[index] as IReadOnlySoundData;
            if (data.Address == address)
            {
                return data;
            }
        }

        return null;
    }

    private List<Data> _dataList;

    //=======================================================
    // editor method
    //=======================================================

    #if UNITY_EDITOR

    public void SetGUID(string guid)
    {
        _guid = guid;
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
