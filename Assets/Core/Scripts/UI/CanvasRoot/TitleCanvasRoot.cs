using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleCanvasRoot : CanvasRoot
{
    /// <summary>
    /// 仮　使用例
    /// </summary>
    [SerializeField]
    TextMeshProUGUI m_titleTxt = null;
    public TextMeshProUGUI titleTxt => m_titleTxt;
}
