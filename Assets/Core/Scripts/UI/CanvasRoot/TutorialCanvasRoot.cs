using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;

public class TutorialCanvasRoot : CanvasRoot
{
    [SerializeField]
    Button m_skipBtn = null;
    public Button skipBtn => m_skipBtn;

    [SerializeField]
    TextMeshProUGUI m_tutorialTxt = null;

    [SerializeField]
    Image m_wazamaeImg = null;

    CanvasGroup m_group = null; // コード書き換えるのが面倒くさいのでCanvasRootでTextフェードしてます

    void Start()
    {
        m_group = m_tutorialTxt.gameObject.GetComponent<CanvasGroup>();
        m_group.alpha = 0f;
        m_tutorialTxt.text = string.Empty;
    }

    /// <summary>
    /// チュートリアルテキストを変更
    /// </summary>
    /// <param name="text">表示するテキスト</param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    public async UniTask ChangeTutorialText(string text, CancellationToken cancellation)
    {
        // 何かテキストが表示されていればフェードアウト
        if (!string.IsNullOrEmpty(m_tutorialTxt.text))
        {
            await m_group.DOFade(0f, 0.2f)
                .WithCancellation(cancellation);
        }
        // テキストを設定
        m_tutorialTxt.text = text;
        // 何かテキストが表示されていればフェードイン
        if (!string.IsNullOrEmpty(m_tutorialTxt.text))
        {
            await m_group.DOFade(1f, 0.2f)
                .WithCancellation(cancellation);
        }
    }

    /// <summary>
    /// ワザマエ!!テクスチャを表示
    /// </summary>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    public async UniTask EnableWazamaeImage(CancellationToken cancellation)
    {
        await m_wazamaeImg.DOFade(1f, 0.2f)
            .WithCancellation(cancellation);
    }
}
