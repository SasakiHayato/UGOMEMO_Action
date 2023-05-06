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

        [SerializeField, ReadOnly] SoundAddress _address;
        [SerializeField] AudioClip _audioClip;
        [SerializeField, Range(0, 1)] private float _volume;
        [SerializeField, Range(-3, 3)] private float _pitch;

        //=======================================================
        // constructor
        //=======================================================

        //=======================================================
        // IReadOnlySoundData interface
        //=======================================================

        AudioClip IReadOnlySoundData.Clip => _audioClip;
        SoundAddress IReadOnlySoundData.Address => _address;
        float IReadOnlySoundData.Volume => _volume;
        float IReadOnlySoundData.Pitch => _pitch;

        #if UNITY_EDITOR
        public void SetSoundAddress(SoundAddress address)
        {
            _address = address;
        }
        #endif
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

    public void Save()
    {
        if (_soundDataList == null) return;
        if (_soundDataList.Count == 0) return;

        _soundDataList.ForEach(data => 
        {
            var read_only_data = data as IReadOnlySoundData;
            for (int index = 0; index < (int)SoundAddress.Length; index++)
            {
                var address = (SoundAddress)index;
                if (address.ToString() == read_only_data.Clip.name)
                {
                    data.SetSoundAddress(address);
                    break;
                }
            }
        });
    }
    #endif
}
