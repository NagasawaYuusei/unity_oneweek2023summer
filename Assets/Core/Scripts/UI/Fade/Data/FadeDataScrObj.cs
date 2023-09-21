using UnityEngine;
using System;
using DG.Tweening;

/// <summary>
/// シーン間フェードデータ
/// </summary>
namespace Data
{
    [CreateAssetMenu(fileName = "FadeData", menuName = "Project/FadeData/FadeData")]
    public class FadeDataScrObj : ScriptableObject
    {
        [SerializeField]
        FadeData m_fadeInData;

        [SerializeField]
        FadeData m_fadeOutData;

        [Serializable]
        public class FadeData
        {
            [SerializeField, Range(0, 1)]
            private float _duration = 0.2f;
            public float duration => _duration;

            [SerializeField]
            private Color _color = Color.black;
            public Color color => _color;

            [SerializeField]
            private float _delay = 0f;
            public float delay =>_delay;

            [SerializeField]
            private Ease _ease = Ease.Linear;
            public Ease ease => _ease;
        }

        /// <summary>
        /// フェードインデータ
        /// </summary>
        public FadeData fadeInData => m_fadeInData;

        /// <summary>
        /// フェードアウトデータ
        /// </summary>
        public FadeData fadeOutData => m_fadeOutData;
    }
}