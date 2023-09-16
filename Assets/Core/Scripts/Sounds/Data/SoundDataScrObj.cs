using UnityEngine;
using System;

/// <summary>
/// サウンドデータ基底クラス
/// </summary>
namespace Data
{
    public abstract class SoundDataScrObj : ScriptableObject
    {
        [Serializable]
        public class SoundData
        {
            public AudioClip AudioClip;
            [Range(0, 1)]
            public float Volume = 1;
        }
    }
}