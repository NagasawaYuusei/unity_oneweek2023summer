using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using KyawaLib;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    /// 背景
    /// </summary>
    Background m_background = null;

    /// <summary>
    /// プレイヤーのHP残量パーセンテージ（クリア時に使用）
    /// </summary>
    public int playerHpPercentage { get; private set; }

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

        await UniTask.DelayFrame(1, cancellationToken: cancellation);

        m_background = GameObject.FindObjectOfType<Background>();
        Debug.Assert(m_background);

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
        // 敵を生成
        var source = CancellationTokenSource.CreateLinkedTokenSource(cancellation);
        EnemySoawnProgress(source.Token).Forget();

        // 終了まで待つ
        await UniTask.WaitUntil(() => isRunning == false, cancellationToken: cancellation);

        // 終了
        await FinalizeAsync(cancellation);
        AudioManager.Instance.FadeOutBgm(0.2f);
        // 敵を生成中ならキャンセル
        source.Cancel();
        source.Dispose();

        // ゲームオーバー処理　クリアも無理やり入れました一緒に使います
        await GameOverProcess(cancellation);

        await FadeManger.instance.Fade(Fade.Situation.Game, Fade.Type.FadeOut);

        // 次のシーンへ
        SceneLoader.instance.LoadMainScene(m_nextScene, cancellation);
        //Destroy(); // ResultSceneManagerでプレイヤーのHP残量を取得してからResult側で破棄
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
    /// 敵生成処理
    /// </summary>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    async UniTask EnemySoawnProgress(CancellationToken cancellation)
    {
        var enemyMng = EnemyManager.instance;
        var cameraZoom = GameObject.FindObjectOfType<CameraZoom>();
        try
        {
            // ボス前のWaveを進行
            var currentWaveCount = 0;
            while (currentWaveCount < enemyMng.wavesCount)
            {
                Debug.Log($"=== Start Wave {currentWaveCount + 1} ===");

                // BGMと背景
                enemyMng.GetBackgroundSpriteAndBGM(currentWaveCount, out var background, out var bgm);
                await m_background.ChangeSky(background, 0.5f, cancellation: cancellation);
                AudioManager.Instance.PlayBGM(bgm);

                // 敵生成開始
                await UniTask.WaitForSeconds(2f, cancellationToken: cancellation);
                enemyMng.StartWave(currentWaveCount);
                // 終了まで待つ
                await UniTask.WaitWhile(() => (enemyMng.isWorking == true), cancellationToken: cancellation);

                // 次のWaveへ
                AudioManager.Instance.FadeOutBgm(0.5f);
                await UniTask.WaitForSeconds(1f, cancellationToken: cancellation);
                currentWaveCount++;
                // カメラ演出
                var posTask = cameraZoom.SetCameraPosition(currentWaveCount, cancellation);
                var zoomTask = cameraZoom.SetCameraSize(currentWaveCount, cancellation);
                await UniTask.WhenAll(posTask, zoomTask);

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: cancellation);
            }
            // ボスWaveを進行
            {
                Debug.Log("=== Start Boss Wave ===");

                // BGMと背景
                enemyMng.GetBossBackgroundSpriteAndBGM(out var background, out var bgm);
                await m_background.ChangeSky(background, 0.5f, cancellation: cancellation);
                AudioManager.Instance.PlayBGM(bgm);

                // ボス生成
                await UniTask.WaitForSeconds(2f, cancellationToken: cancellation);
                enemyMng.StartBossWave();
                // 終了まで待つ
                await UniTask.WaitUntil(() => (enemyMng.isWorking == false), cancellationToken: cancellation);
            }
            // ここまできたらクリア
            OnGameClear();
        }
        catch
        {
            enemyMng.StopWave();
        }
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    /// <returns></returns>
    async UniTask GameOverProcess(CancellationToken cancellation)
    {
        bool isWaiting = true;

        var overScene = await SceneLoader.instance.LoadSubSceneAsync(SceneIndex.Sub.GameOver);
        SceneManager.SetActiveScene(overScene);

        await UniTask.DelayFrame(1, cancellationToken: cancellation);

        var overCanvasRoot = GameObject.FindObjectOfType<GameOverCanvasRoot>();
        Debug.Assert(overCanvasRoot);

        // クリア用
        if (m_state == GameState.Clear)
        {
            overCanvasRoot.SetClear();
            await UniTask.WaitForSeconds(2f, cancellationToken: cancellation);
            return;
        }

        overCanvasRoot.titleBtn.onClick.AddListener(
            () =>
            {
                m_nextScene = SceneIndex.Main.Title;
                isWaiting = false;
            });
        overCanvasRoot.gameBtn.onClick.AddListener(
            () =>
            {
                m_nextScene = SceneIndex.Main.Game;
                isWaiting = false;
            });

        // プレイヤー入力待ち
        await UniTask.WaitUntil(() => isWaiting == false, cancellationToken: cancellation);
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
        playerHpPercentage = PlayerController.Instance.GetPlayerHpPercentage();
    }
}
