using UnityEngine;
using Data;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioSource _audioBGM;
    [SerializeField] AudioSource _audioSE;

    [SerializeField] BGMSoundDataScrObj _bgmData;
    [SerializeField] SESoundDataScrObj _seData;

    [SerializeField]
    private float _masterVolume = 1;
    [SerializeField]
    private float _bgmMasterVolume = 1;
    [SerializeField]
    private float _seMasterVolume = 1;

    /// <summary>
    /// BGM音量
    /// </summary>
    public float bgmMasterVolume
    {
        get { return _bgmMasterVolume; }
        set { _bgmMasterVolume = value; }
    }
    /// <summary>
    /// SE音量
    /// </summary>
    public float seMasterVolume
    {
        get { return _seMasterVolume; }
        set { _seMasterVolume = value; }
    }

    void Awake()
    {
        if (Instance)
        {
            Debug.LogWarning("AudioManager������������[");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// BGM���Đ�
    /// </summary>
    /// <param name="bgm">�Đ�������BGM</param>
    public void PlayBGM(SoundType.BGM bgm)
    {
        var data = _bgmData.GetSoundData(bgm);
        if (data == null)
            return;

        _audioBGM.clip = data.AudioClip;
        _audioBGM.volume = data.Volume * _bgmMasterVolume * _masterVolume;
        _audioBGM.Play();
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    public void StopBgm()
    {
        _audioBGM.Stop();
    }

    /// <summary>
    /// SE���Đ�
    /// </summary>
    /// <param name="se">�Đ�������SE</param>
    public void PlaySE(SoundType.SE se)
    {
        var data = _seData.GetSoundData(se);
        if (data == null)
            return;

        _audioSE.volume = data.Volume * _seMasterVolume * _masterVolume;
        _audioSE.PlayOneShot(data.AudioClip);
    }

    /// <summary>
    /// SE停止
    /// </summary>
    public void StopSE()
    {
        _audioSE.Stop();
    }
}
