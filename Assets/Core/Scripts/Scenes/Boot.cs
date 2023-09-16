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
    const int BUILD_INDEX_TITLE = 1;

#if UNITY_EDITOR
    static int ms_firstSceneIndex = 0;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeOnLoad()
    {
        // エディタで開いているシーンを取得します
        var firstScene = SceneManager.GetActiveScene();
        ms_firstSceneIndex = firstScene.buildIndex;

        Debug.Log($"Start with {firstScene.name} scene.");

        // まずBootシーンから開始します
        if (ms_firstSceneIndex != BUILD_INDEX_BOOT)
        {
            SceneManager.LoadScene(BUILD_INDEX_BOOT);
        }
    }
#endif

    async void Start()
    {
        // 常駐
        DontDestroyOnLoad(gameObject);
        // ゲームのセットアップ
        await InitializeAsync();

#if UNITY_EDITOR
        if (ms_firstSceneIndex != BUILD_INDEX_BOOT)
        {
            // 開始時にエディタで開いていたシーンから開始します
            SceneManager.LoadScene(ms_firstSceneIndex);
            return;
        }
#endif
        // Titleから開始します
        SceneManager.LoadScene(BUILD_INDEX_TITLE);
    }

    async UniTask InitializeAsync()
    {
        // ここにゲーム内で常駐させたいものの初期化処理を書きます

        await UniTask.CompletedTask;
    }
}
