using System.Collections.Generic;
using UnityEngine;
using PoolSystem;

/// <summary>
/// �T�E���h�Ǘ��N���X
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
    /// �T�E���h�̍Đ�
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
            Debug.LogWarning("��v����T�E���h�f�[�^��������܂���ł����B�������X�L�b�v���܂��B");
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
    /// ����BGM�̒�~
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
    /// ������
    /// </summary>
    protected override void Initialize()
    {
        
    }
}
