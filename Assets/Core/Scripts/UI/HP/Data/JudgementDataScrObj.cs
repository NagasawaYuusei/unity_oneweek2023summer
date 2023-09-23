using System;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// 判定パーセンテージとスプライトのデータ
    /// </summary>
    [CreateAssetMenu(fileName = "JudgementData", menuName = "Project/JudgementData")]
    public class JudgementDataScrObj : ScriptableObject
    {
        [SerializeField]
        Rank[] m_ranks = null;

        [Serializable]
        class Rank
        {
            [SerializeField, Range(0, 100)]
            private int _maxPercentage = 0;
            public int maxPercentage => _maxPercentage;

            [SerializeField]
            private Sprite _sprite = null;
            public Sprite sprite => _sprite;
        }

#if false
    void Start()
    {
        // 判定テスト
        var percentages = new int[] { 0, 10, 19, 20, 50, 79, 80, 81, 99, 100 };
        foreach (var per in percentages)
        {
            var sprite = GetJudgementSprite(per);
            Debug.Log(per + " : " + sprite.name);
        }
    ;}
#endif

        /// <summary>
        /// パーセンテージに応じた判定スプライトを取得
        /// </summary>
        /// <param name="percentage">プレイヤーのHP残量パーセンテージ</param>
        /// <returns>判定スプライト</returns>
        public Sprite GetJudgementSprite(int percentage)
        {
            if (m_ranks == null)
                return null;

            Sprite sprite = null;
            foreach (var rank in m_ranks)
            {
                if (percentage <= rank.maxPercentage)
                {
                    sprite = rank.sprite;
                    break;
                }
            }
            return sprite;
        }
    }
}