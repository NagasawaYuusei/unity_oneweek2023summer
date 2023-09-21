using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームのセットアップを行うクラス
/// 必ず最初に実行されます
/// </summary>
public class Boot : MonoBehaviour
{
    const int BUILD_INDEX_BOOT = 0;

    CancellationToken m_cancellation;

#if UNITY_EDITOR
    static int ms_firstSceneIndex = 0;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeOnLoad()
    {
        // エディタで開いているシーンを取得します
        var firstScene = SceneManager.GetActiveScene();
        Debug.Log($"Start with {firstScene.name} scene.");

        if (firstScene.buildIndex == BUILD_INDEX_BOOT)
        {
            // エディタで開いているシーンがBootならreturn
            return;
        }
        else if (firstScene.name == "Start")
        {
            // 非プログラマーが開いているStartシーンから始まった場合、通常進行で開始します
            ms_firstSceneIndex = BUILD_INDEX_BOOT;
        }
        else
        {
            // エディタで開いているシーンから開始します
            ms_firstSceneIndex = firstScene.buildIndex;
        }
        // まずBootシーンから開始します
        SceneManager.LoadScene(BUILD_INDEX_BOOT);
    }
#endif

    void Awake()
    {
        m_cancellation = gameObject.GetCancellationTokenOnDestroy();
        // 常駐
        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        // ゲームのセットアップ
        await InitializeAsync();

#if UNITY_EDITOR
        if (ms_firstSceneIndex != BUILD_INDEX_BOOT)
        {
            // 開始時にエディタで開いていたシーンから開始します
            bool isMainScene = SceneLoader.instance.LoadMainScene((SceneIndex.Main)ms_firstSceneIndex, m_cancellation);
            if (!isMainScene)
                SceneManager.LoadScene(ms_firstSceneIndex);
            return;
        }
#endif
        // Titleから開始します
        await FadeManger.instance.fader.FadeOut(0);
        SceneLoader.instance.LoadMainScene(SceneIndex.Main.Title, m_cancellation);

    }

    async UniTask InitializeAsync()
    {
        // ここにゲーム内で常駐させたいものの初期化処理を書きます

        SceneLoader.Create(gameObject);

        FadeManger.instance.Initialize();

        await UniTask.CompletedTask;
    }
}
