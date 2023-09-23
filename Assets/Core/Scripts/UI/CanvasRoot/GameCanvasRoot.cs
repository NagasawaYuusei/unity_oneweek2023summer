using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameCanvasRoot : CanvasRoot
{
    [SerializeField]
    Image m_fillImg = null;

    /// <summary>
    /// HPバーを設定
    /// </summary>
    /// <param name="fillAmount"></param>
    public void SetHpFillAmount(float fillAmount)
    {
        m_fillImg.fillAmount = fillAmount;
    }
}
