using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using KyawaLib;
using UnityEngine.SceneManagement;

public class GameSceneManager : SingletonClass<GameSceneManager>
{
    GameCanvasRoot m_canvasRoot = null;
    bool m_isRunning = false;

    /// <summary>
    /// UI参照
    /// </summary>
    public GameCanvasRoot canvasRoot => m_canvasRoot;

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
        // サブシーンをロード
        var scene = await SceneLoader.instance.LoadSubSceneAsync(SceneIndex.Sub.Stage000);
        SceneManager.SetActiveScene(scene);

        await UniTask.DelayFrame(1);

        m_canvasRoot = GameObject.FindObjectOfType<GameCanvasRoot>();
        Debug.Assert(m_canvasRoot);
        m_canvasRoot.nextBtn.onClick.AddListener(() => m_isRunning = false);
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
        await UniTask.WaitUntil(() => m_isRunning == false);

        m_isRunning = false;

        // 終了
        await FinalizeAsync(cancellation);

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
