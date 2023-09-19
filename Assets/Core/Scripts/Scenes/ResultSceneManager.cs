using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using KyawaLib;

public class ResultSceneManager : SingletonClass<ResultSceneManager>
{
    ResultCanvasRoot m_canvasRoot = null;
    bool m_isRunning = false;

    /// <summary>
    /// 次に遷移するシーン
    /// </summary>
    SceneIndex.Main m_nextScene = SceneIndex.Main.Title;

    /// <summary>
    /// UI参照
    /// </summary>
    public ResultCanvasRoot canvasRoot => m_canvasRoot;

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
        /* 初期化処理 */
        await UniTask.Delay(1);

        m_canvasRoot = GameObject.FindObjectOfType<ResultCanvasRoot>();
        Debug.Assert(m_canvasRoot);
        m_canvasRoot.titleBtn.onClick.AddListener(
            () =>
            {
                m_nextScene = SceneIndex.Main.Title;
                m_isRunning = false;
            });
        m_canvasRoot.gameBtn.onClick.AddListener(
            () =>
            {
                m_nextScene = SceneIndex.Main.Game;
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

        m_isRunning = true;

        /* シーン内処理 */
        await UniTask.WaitUntil(() => m_isRunning == false, cancellationToken: cancellation);

        m_isRunning = false;

        // 終了
        await FinalizeAsync(cancellation);

        // 次のシーンへ
        SceneLoader.instance.LoadMainScene(m_nextScene, cancellation);
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
