using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] SoundPlayer _sounder;
    [SerializeField] SoundAddress _bgmSoundPath = SoundAddress.None;
    [SerializeField] List<SoundDataAsset> _soundDataAssetList;

    public SoundPlayer Sounder => _sounder;
    public SoundAddress SoundAddress => _bgmSoundPath;
    public IReadOnlyList<SoundDataAsset> SoundDataAsset => _soundDataAssetList;

    private void Start()
    {
        SoundManager.Instance.SetController(this);
        
    }

    public void Play()
    {
        SoundManager.Instance.Play(Define.Sound.Type.BGM, _bgmSoundPath);
    }
}
