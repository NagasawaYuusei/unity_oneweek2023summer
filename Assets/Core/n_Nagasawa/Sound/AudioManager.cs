using UnityEngine;
using Data;
using System.Collections;

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
    /// BGMをフェードアウト中か
    /// </summary>
    bool _onFadeOutBGM = false;

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

        _onFadeOutBGM = false;

        _audioBGM.clip = data.AudioClip;
        _audioBGM.volume = data.Volume * _bgmMasterVolume * _masterVolume;
        _audioBGM.Play();
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    public void StopBgm()
    {
        _onFadeOutBGM = false;
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

    /// <summary>
    /// BGMをフェードアウトして停止
    /// フェードアウト中に再びこの関数が呼ばれた場合無視されます。
    /// </summary>
    /// <param name="sec">フェードアウト秒数</param>
    public void FadeOutBgm(float sec)
    {
        if (_onFadeOutBGM)
            return;

        if (sec <= 0f)
            StopBgm();
        else
            StartCoroutine(CoFadeOutBGM(sec));
    }

    IEnumerator CoFadeOutBGM(float sec)
    {
        _onFadeOutBGM = true;

        float startVolume = _audioBGM.volume;
        float time = sec;
        while (0f < time)
        {
            time -= Time.deltaTime;
            _audioBGM.volume = startVolume * (time / sec);
            yield return null;
        }
        StopBgm();
    }
}
