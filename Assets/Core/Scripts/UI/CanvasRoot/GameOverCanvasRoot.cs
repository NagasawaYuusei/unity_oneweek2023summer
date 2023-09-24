using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverCanvasRoot : CanvasRoot
{
    [SerializeField]
    Button m_titleBtn = null;
    public Button titleBtn => m_titleBtn;

    [SerializeField]
    Button m_gameBtn = null;
    public Button gameBtn => m_gameBtn;

    [SerializeField]
    GameObject m_mojiLose = null;
    [SerializeField]
    GameObject m_mojiWin = null;

    /// <summary>
    /// ゲームオーバーUIをゲームクリアUIとして無理やり使います
    /// </summary>
    public void SetClear()
    {
        m_mojiWin.SetActive(true);
        m_mojiLose.SetActive(false);
        m_titleBtn.gameObject.SetActive(false);
        m_gameBtn.gameObject.SetActive(false);
    }
}
