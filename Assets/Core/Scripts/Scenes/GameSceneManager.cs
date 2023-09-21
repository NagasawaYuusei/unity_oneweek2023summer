using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using KyawaLib;
using UnityEngine.SceneManagement;

public class GameSceneManager : SingletonClass<GameSceneManager>
{
    GameCanvasRoot m_canvasRoot = null;
    GameBackCanvasRoot m_backCanvasRoot = null;

    enum GameState
    {
        Loading,
        Playing,
        Clear,
        Over,
    }
    GameState m_state = GameState.Loading;

    /// <summary>
    /// 次に遷移するシーン
    /// </summary>
    SceneIndex.Main m_nextScene = SceneIndex.Main.Title;

    /// <summary>
    /// GameシーンのUI参照
    /// </summary>
    public GameCanvasRoot canvasRoot => m_canvasRoot;
    /// <summary>
    /// GameBackシーンのUI参照
    /// </summary>
    public GameBackCanvasRoot backCanvasRoot => m_backCanvasRoot;

    /// <summary>
    /// 実行中か
    /// </summary>
    public bool isRunning => (m_state == GameState.Playing);

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    async UniTask InitializeAsync(CancellationToken cancellation)
    {
        // サブシーンをロード
        var backScene = await SceneLoader.instance.LoadSubSceneAsync(SceneIndex.Sub.GameBack);
        var enemyScene = await SceneLoader.instance.LoadSubSceneAsync(SceneIndex.Sub.GameEnemy);
        SceneManager.SetActiveScene(backScene);
        SceneManager.SetActiveScene(enemyScene);

        await UniTask.DelayFrame(1);

        m_canvasRoot = GameObject.FindObjectOfType<GameCanvasRoot>();
        Debug.Assert(m_canvasRoot);

        m_backCanvasRoot = GameObject.FindObjectOfType<GameBackCanvasRoot>();
        Debug.Assert(m_backCanvasRoot);
        m_backCanvasRoot.clearBtn.onClick.AddListener(
            () =>
            {
                OnGameClear();
            });
        m_backCanvasRoot.gameoverBtn.onClick.AddListener(
            () =>
            {
                OnGameOver();
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
        await FadeManger.instance.Fade(Fade.Situation.Game, Fade.Type.FadeIn);

        m_state = GameState.Playing;

        /* シーン内処理 */
        await UniTask.WaitUntil(() => isRunning == false, cancellationToken: cancellation);

        // 終了
        await FinalizeAsync(cancellation);

        // ゲームオーバー処理
        if (m_state == GameState.Over)
        {
            await GameOverProcess(cancellation);
        }
        await FadeManger.instance.Fade(Fade.Situation.Game, Fade.Type.FadeOut);

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

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    /// <returns></returns>
    async UniTask GameOverProcess(CancellationToken cancellation)
    {
        bool isRunning = true;

        var overScene = await SceneLoader.instance.LoadSubSceneAsync(SceneIndex.Sub.GameOver);
        SceneManager.SetActiveScene(overScene);

        await UniTask.DelayFrame(1);

        var overCanvasRoot = GameObject.FindObjectOfType<GameOverCanvasRoot>();
        Debug.Assert(overCanvasRoot);
        overCanvasRoot.titleBtn.onClick.AddListener(
            () =>
            {
                m_nextScene = SceneIndex.Main.Title;
                isRunning = false;
            });
        overCanvasRoot.gameBtn.onClick.AddListener(
            () =>
            {
                m_nextScene = SceneIndex.Main.Game;
                isRunning = false;
            });

        // プレイヤー入力待ち
        await UniTask.WaitUntil(() => isRunning == false, cancellationToken: cancellation);
    }

    /// <summary>
    /// プレイヤーが死亡した（ゲームオーバー）
    /// </summary>
    public void OnGameOver()
    {
        m_state = GameState.Over;
    }

    /// <summary>
    /// ボスが死亡した（ゲームクリア）
    /// </summary>
    public void OnGameClear()
    {
        m_state = GameState.Clear;
        m_nextScene = SceneIndex.Main.Result;
    }
}
