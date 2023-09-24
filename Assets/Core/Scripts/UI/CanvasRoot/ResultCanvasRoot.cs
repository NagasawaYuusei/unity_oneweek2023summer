using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Data;

public class ResultCanvasRoot : CanvasRoot
{
    [SerializeField]
    Button m_titleBtn = null;
    public Button titleBtn => m_titleBtn;

    [SerializeField]
    Button m_gameBtn = null;
    public Button gameBtn => m_gameBtn;

    [SerializeField]
    Image m_rankImg = null;

    [SerializeField]
    JudgementDataScrObj m_judgementData = null;

    /// <summary>
    /// 判定スプライトを設定
    /// </summary>
    /// <param name="percentage">プレイヤーのHPの残量パーセンテージ</param>
    public void SetRankSprite(int percentage)
    {
        m_rankImg.sprite = m_judgementData.GetJudgementSprite(percentage);
    }
}
