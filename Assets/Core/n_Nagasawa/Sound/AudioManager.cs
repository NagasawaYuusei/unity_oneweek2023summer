using UnityEngine;
using Data;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioSource _audioBGM;
    [SerializeField] AudioSource _audioSE;
    [SerializeField] AudioSource _audioBGM_intro;

    [SerializeField] BGMSoundDataScrObj _bgmData;
    [SerializeField] SESoundDataScrObj _seData;

    [SerializeField]
    private float _masterVolume = 1;
    [SerializeField]
    private float _bgmMasterVolume = 1;
    [SerializeField]
    private float _seMasterVolume = 1;

    float _currentSeDataVolume = 1f;
    float _currentBgmDataVolume = 1f;

    /// <summary>
    /// BGMをフェードアウト中か
    /// </summary>
    bool _onFadeOutBGM = false;

    /// <summary>
    /// BGM音量
    /// </summary>
    public float bgmMasterVolume
    {
        get { return _audioBGM.volume; }
        set
        {
            _bgmMasterVolume = value;
            UpdateBgmVolume();
        }
    }
    /// <summary>
    /// SE音量
    /// </summary>
    public float seMasterVolume
    {
        get { return _audioSE.volume; }
        set
        {
            _seMasterVolume = value;
            UpdateSeVolume();
        }
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

    void UpdateBgmVolume()
    {
        var volume = _currentBgmDataVolume * _bgmMasterVolume * _masterVolume; ;
        _audioBGM.volume = volume;
        _audioBGM_intro.volume = volume;
    }
    void UpdateSeVolume()
    {
        _audioSE.volume = _currentSeDataVolume * _seMasterVolume * _masterVolume;
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
        _currentBgmDataVolume = data.Volume;
        UpdateBgmVolume();

        if (data.IntroAudioClip != null)
        {
            // イントロあり
            _audioBGM_intro.clip = data.IntroAudioClip;
            _audioBGM.clip = data.AudioClip;
            // イントロ再生開始
            _audioBGM_intro.PlayScheduled(AudioSettings.dspTime);
            //イントロ終了後にループ部分の再生を開始
            _audioBGM.PlayScheduled(AudioSettings.dspTime + (_audioBGM_intro.clip.samples / (float)_audioBGM_intro.clip.frequency));
        }
        else
        {
            // イントロなし
            _audioBGM.clip = data.AudioClip;
            _audioBGM.Play();
        }
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    public void StopBgm()
    {
        _onFadeOutBGM = false;
        _audioBGM.Stop();
        _audioBGM_intro.Stop();
        _currentSeDataVolume = 1f;
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

        _currentSeDataVolume = data.Volume;
        UpdateSeVolume();
        _audioSE.PlayOneShot(data.AudioClip);
    }

    /// <summary>
    /// SE停止
    /// </summary>
    public void StopSE()
    {
        _audioSE.Stop();
        _currentSeDataVolume = 1f;
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
            var volume = startVolume *(time / sec);
            _audioBGM.volume = volume;
            _audioBGM_intro.volume = startVolume;
            yield return null;
        }
        StopBgm();
    }
}
