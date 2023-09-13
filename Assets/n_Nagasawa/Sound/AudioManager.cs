using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioSource _audioBGM;
    [SerializeField] AudioSource _audioSE;

    [SerializeField] List<BGMSoundData> _bgmSoundDatas;
    [SerializeField] List<SESoundData> _seSoundDatas;

    [SerializeField]
    private float _masterVolume = 1;
    [SerializeField]
    private float _bgmMasterVolume = 1;
    [SerializeField]
    private float _seMasterVolume = 1;

    void Awake()
    {
        if (Instance)
        {
            Debug.LogWarning("AudioManagerï°êîÇ†Ç¡ÇΩÇÊÅ[");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// BGMÇçƒê∂
    /// </summary>
    /// <param name="bgm">çƒê∂ÇµÇΩÇ¢BGM</param>
    public void PlayBGM(BGMSoundData.BGM bgm)
    {
        BGMSoundData data = _bgmSoundDatas.Find(data => data._bgm == bgm);
        _audioBGM.clip = data._audioClip;
        _audioBGM.volume = data._volume * _bgmMasterVolume * _masterVolume;
        _audioBGM.Play();
    }

    /// <summary>
    /// SEÇçƒê∂
    /// </summary>
    /// <param name="se">çƒê∂ÇµÇΩÇ¢SE</param>
    public void PlaySE(SESoundData.SE se)
    {
        SESoundData data = _seSoundDatas.Find(data => data.Se == se);
        _audioSE.volume = data.Volume * _seMasterVolume * _masterVolume;
        _audioSE.PlayOneShot(data.AudioClip);
    }

    [System.Serializable]
    public class BGMSoundData
    {
        public enum BGM
        {
            Title,
            Game,
            Result,
        }

        public BGM _bgm;
        public AudioClip _audioClip;
        [Range(0f, 1f)]
        public float _volume = 1f;
    }

    [System.Serializable]
    public class SESoundData
    {
        public enum SE
        {
            Start,
            CountDown,
            End,
            ClickButton,
            ChangeScene
        }

        public SE Se;
        public AudioClip AudioClip;
        [Range(0, 1)]
        public float Volume = 1;
    }
}
