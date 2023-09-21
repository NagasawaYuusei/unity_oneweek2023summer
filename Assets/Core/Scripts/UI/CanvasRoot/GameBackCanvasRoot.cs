using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameBackCanvasRoot : CanvasRoot
{
    [SerializeField]
    Button m_clearBtn = null;
    public Button clearBtn => m_clearBtn;

    [SerializeField]
    Button m_gameoverBtn = null;
    public Button gameoverBtn => m_gameoverBtn;
}
