using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameCanvasRoot : CanvasRoot
{
    [SerializeField]
    Button m_nextBtn = null;
    public Button nextBtn => m_nextBtn;
}
