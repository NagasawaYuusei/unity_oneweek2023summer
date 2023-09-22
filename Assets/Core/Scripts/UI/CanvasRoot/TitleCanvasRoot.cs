using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TitleCanvasRoot : CanvasRoot
{
    [SerializeField]
    Button m_optionBtn = null;
    /// <summary>
    /// オプションボタン
    /// </summary>
    public Button optionBtn => m_optionBtn;
}
