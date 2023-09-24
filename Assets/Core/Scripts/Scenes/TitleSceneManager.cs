using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using KyawaLib;
using UnityEngine.SceneManagement;

public class TitleSceneManager : SingletonClass<TitleSceneManager>
{
    TitleCanvasRoot m_canvasRoot = null;
    bool m_isRunning = false;
    bool m_canInput = false;

    /// <summary>
    /// UI参照
    /// </summary>
    public TitleCanvasRoot canvasRoot => m_canvasRoot;

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

        m_canvasRoot = GameObject.FindObjectOfType<TitleCanvasRoot>();
        Debug.Assert(m_canvasRoot);
        // オプションボタン
        m_canvasRoot.optionBtn.onClick.AddListener(
            () =>
            {
                OpenyOption(cancellation).Forget();
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
        await FadeManger.instance.Fade(Fade.Situation.Title, Fade.Type.FadeIn);
        // BGM開始
        AudioManager.Instance.PlayBGM(SoundType.BGM.FirstWave);

        m_isRunning = true;
        m_canInput = true;

        /* シーン内処理 */
        while (m_isRunning)
        {
            if (m_canInput)
            {
                if (Input.anyKeyDown
                    && !Input.GetMouseButton(0)
                    && !Input.GetMouseButton(1)
                    && !Input.GetMouseButton(2))
                {
                    break;
                }
            }
            await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: cancellation);
        }
        m_isRunning = false;
        m_canInput = false;

        // 終了
        await FinalizeAsync(cancellation);
        await FadeManger.instance.Fade(Fade.Situation.Title, Fade.Type.FadeOut);

        // 次のシーンへ
        SceneLoader.instance.LoadMainScene(SceneIndex.Main.Game, cancellation);
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

    async UniTask OpenyOption(CancellationToken cancellation)
    {
        m_canInput = false;
        bool onClose = false;

        // オプションシーンをロード
        var optionScene = await SceneLoader.instance.LoadSubSceneAsync(SceneIndex.Sub.TitleOption);
        await UniTask.DelayFrame(1);

        var optionCanvasRoot = GameObject.FindObjectOfType<OptionCanvasRoot>();
        Debug.Assert(optionCanvasRoot);
        // SEスライダー
        optionCanvasRoot.seSlider.value = AudioManager.Instance.seMasterVolume;
        optionCanvasRoot.seSlider.onValueChanged.AddListener(
            (value) =>
            {
                AudioManager.Instance.seMasterVolume = value;
            });
        // BGMスライダー
        optionCanvasRoot.bgmSlider.value = AudioManager.Instance.bgmMasterVolume;
        optionCanvasRoot.bgmSlider.onValueChanged.AddListener(
            (value) =>
            {
                AudioManager.Instance.bgmMasterVolume = value;
            });
        // 戻るボタン
        optionCanvasRoot.backBtn.onClick.AddListener(
            () =>
            {
                // SE
                AudioManager.Instance.PlaySE(SoundType.SE.Select);
                onClose = true;
            });

        // シーン表示
        SceneManager.SetActiveScene(optionScene);
        // 開く
        await optionCanvasRoot.FadeIn(cancellation);
        // 戻るボタンが押されるまで待つ
        await UniTask.WaitUntil(() => (onClose == true), cancellationToken: cancellation);
        // 閉じる
        await optionCanvasRoot.FadeOut(cancellation);
        // オプションシーンをアンロード
        await SceneLoader.instance.UnloadSubSceneAsync(optionScene, false);

        m_canInput = true;
    }
}
