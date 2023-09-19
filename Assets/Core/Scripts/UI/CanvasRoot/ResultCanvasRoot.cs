using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultCanvasRoot : CanvasRoot
{
    [SerializeField]
    Button m_titleBtn = null;
    public Button titleBtn => m_titleBtn;

    [SerializeField]
    Button m_gameBtn = null;
    public Button gameBtn => m_gameBtn;
}
