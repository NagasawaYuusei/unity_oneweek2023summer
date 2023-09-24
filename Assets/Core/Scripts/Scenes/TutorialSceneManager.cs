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
        await UniTask.DelayFrame(1, cancellationToken: cancellation);

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

        // チュートリアル処理
        var source = CancellationTokenSource.CreateLinkedTokenSource(cancellation);
        TutorialProcess(source.Token).Forget();

        // 終了まで待つ
        await UniTask.WaitUntil(() => isRunning == false, cancellationToken: cancellation);
        // チュートリアル中ならキャンセル
        source?.Cancel();
        source?.Dispose();

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

    async UniTask TutorialProcess(CancellationToken cancellation)
    {
        // チュートリアルテキストを表示
        await m_canvasRoot.ChangeTutorialText("攻撃を受けろ！\n[space]", cancellation);
        await UniTask.WaitForSeconds(0.8f, cancellationToken: cancellation);
        // チュートリアルの敵とプレイヤーを取得
        var enemy = GameObject.FindObjectOfType<TutorialEnemy>();
        var player = PlayerController.Instance;
        // 敵が刀を振り上げる
        await UniTask.WaitForSeconds(0.8f, cancellationToken: cancellation);
        enemy.OnAnticipation();
        await UniTask.WaitForSeconds(0.5f, cancellationToken: cancellation);
        // 敵が刀を振り下ろす
        enemy.OnAttack();
        while (true)
        {
            if (player.state == PlayerController.PlayerStateEnum.ParrySuccess)
            {
                // 成功　敵が死んでチュートリアル終了
                enemy.OnDeath();
                await m_canvasRoot.ChangeTutorialText("ワザマエ!!", cancellation);
                await UniTask.WaitForSeconds(3f, cancellationToken: cancellation);
                m_isRunning = false;
                break;
            }
            else if (player.state == PlayerController.PlayerStateEnum.TakeHit)
            {
                // 失敗　チュートリアルもう一度
                await m_canvasRoot.ChangeTutorialText("もう一度", cancellation);
                await UniTask.WaitForSeconds(0.5f, cancellationToken: cancellation);
                // 入力があるまで待つ
                await UniTask.WaitUntil(() => (Input.anyKeyDown == true), cancellationToken: cancellation);
                enemy.Reset();
                TutorialProcess(cancellation).Forget();
                break;
            }
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: cancellation);
        }

    }
}
