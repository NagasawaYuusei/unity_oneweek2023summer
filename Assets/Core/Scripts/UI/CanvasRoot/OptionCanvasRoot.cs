using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;

public class OptionCanvasRoot : CanvasRoot
{
    [SerializeField]
    Button m_backBtn = null;
    /// <summary>
    /// タイトルに戻るボタン
    /// </summary>
    public Button backBtn => m_backBtn;

    [SerializeField]
    Slider m_seSlider = null;
    /// <summary>
    /// SE音量調整スライダー
    /// </summary>
    public Slider seSlider => m_seSlider;

    [SerializeField]
    Slider m_bgmSlider = null;
    /// <summary>
    /// BGM音量調整スライダー
    /// </summary>
    public Slider bgmSlider => m_bgmSlider;

    CanvasGroup m_group = null;

    void Awake()
    {
        m_group = GetComponent<CanvasGroup>();
        m_group.alpha = 0f;
        SetEnable(false);
    }

    void SetEnable(bool onEnable)
    {
        backBtn.enabled = onEnable;
        m_seSlider.enabled = onEnable;
        m_bgmSlider.enabled = onEnable;
    }

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    public async UniTask FadeIn(CancellationToken cancellation)
    {
        await m_group.DOFade(1f, 0.2f)
            .WithCancellation(cancellation);
        SetEnable(true);
    }

    /// <summary>
    /// 非表示
    /// </summary>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    public async UniTask FadeOut(CancellationToken cancellation)
    {
        SetEnable(false);
        await m_group.DOFade(0f, 0.2f)
            .WithCancellation(cancellation);
    }
}
