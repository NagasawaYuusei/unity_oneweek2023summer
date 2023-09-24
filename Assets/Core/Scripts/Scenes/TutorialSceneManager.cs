using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using KyawaLib;
using UnityEngine.SceneManagement;

public class TutorialSceneManager : SingletonClass<TutorialSceneManager>
{
    TutorialCanvasRoot m_canvasRoot = null;
    bool m_isRunning = false;

    /// <summary>
    /// UI参照
    /// </summary>
    public TutorialCanvasRoot canvasRoot => m_canvasRoot;

    /// <summary>
    /// 実行中か
    /// </summary>
    public bool isRunning => m_isRunning;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    async UniTask InitializeAsync(CancellationToken cancellation)
    {
        await UniTask.DelayFrame(1);

        m_canvasRoot = GameObject.FindObjectOfType<TutorialCanvasRoot>();
        Debug.Assert(m_canvasRoot);
        // スキップボタン
        m_canvasRoot.skipBtn.onClick.AddListener(
            () =>
            {
                m_isRunning = false;
            });
    }

    /// <summary>
    /// シーン開始
    /// </summary>
    /// <returns></returns>
    public async UniTask Run(CancellationToken cancellation)
    {
        // 初期化
        await InitializeAsync(cancellation);
        await FadeManger.instance.Fade(Fade.Situation.Tutorial, Fade.Type.FadeIn);

        m_isRunning = true;

        // 終了まで待つ
        await UniTask.WaitUntil(() => isRunning == false, cancellationToken: cancellation);

        // 終了
        await FinalizeAsync(cancellation);
        await FadeManger.instance.Fade(Fade.Situation.Tutorial, Fade.Type.FadeOut);

        // 次のシーンへ
        SceneLoader.instance.LoadMainScene(SceneIndex.Main.Title, cancellation);
        Destroy();
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    /// <returns></returns>
    async UniTask FinalizeAsync(CancellationToken cancellation)
    {
        /* 終了処理 */
        await UniTask.CompletedTask;
    }
}
