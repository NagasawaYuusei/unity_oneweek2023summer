using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Canvas直下のオブジェクトにアタッチします
/// Canvasの設定を楽にしたり、継承してUIオブジェクトの参照を簡単にしたりするために使用します
/// </summary>
public abstract class CanvasRoot : MonoBehaviour
{
    protected Canvas Canvas { get; private set; } = null;

    protected CanvasScaler CanvasScaler { get; private set; } = null;

    protected GraphicRaycaster GraphicRaycaster { get; private set; } = null;

    void Awake()
    {
        Canvas = GetComponentInParent<Canvas>();
        CanvasScaler = GetComponentInParent<CanvasScaler>();
        GraphicRaycaster = GetComponentInParent<GraphicRaycaster>();
    }
}