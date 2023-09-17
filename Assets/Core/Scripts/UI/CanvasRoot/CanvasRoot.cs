using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Canvas直下のオブジェクトにアタッチします
/// Canvasの設定を楽にしたり、継承してUIオブジェクトの参照を簡単にしたりするために使用します
/// </summary>
public abstract class CanvasRoot : MonoBehaviour
{
    protected Canvas canvas { get; private set; } = null;

    protected CanvasScaler canvasScaler { get; private set; } = null;

    protected GraphicRaycaster graphicRaycaster { get; private set; } = null;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasScaler = GetComponentInParent<CanvasScaler>();
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
    }
}