using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンロード処理
/// 内部コルーチンを使用するためSingletonMonoBehaviourにします.
/// 処理中にゲームオブジェクトが破棄されるとキャンセルされます.
/// </summary>
public class SceneLoader : KyawaLib.SingletonMonoBehaviour<SceneLoader>
{
    /// <summary>
    /// メインシーンを同期ロード
    /// </summary>
    /// <param name="name">シーンインデックス</param>
    public bool LoadMainScene(SceneIndex.Main name, CancellationToken cancellation)
    {
        Debug.Log($"Load {name} scene.");
        switch (name)
        {
            case SceneIndex.Main.Title:
                {
                    TitleSceneManager.instance?.Destroy();
                    var manager = TitleSceneManager.Create();
                    SceneManager.LoadScene((int)name, LoadSceneMode.Single);
                    manager.Run(cancellation).Forget();
                    return true;
                }
            case SceneIndex.Main.Game:
                {
                    GameSceneManager.instance?.Destroy();
                    var manager = GameSceneManager.Create();
                    SceneManager.LoadScene((int)name, LoadSceneMode.Single);
                    manager.Run(cancellation).Forget();
                    return true;
                }
            case SceneIndex.Main.Result:
                {
                    ResultSceneManager.instance?.Destroy();
                    var manager = ResultSceneManager.Create();
                    SceneManager.LoadScene((int)name, LoadSceneMode.Single);
                    manager.Run(cancellation).Forget();
                    return true;
                }
            case SceneIndex.Main.Tutorial:
                {
                    TutorialSceneManager.instance?.Destroy();
                    var manager = TutorialSceneManager.Create();
                    SceneManager.LoadScene((int)name, LoadSceneMode.Single);
                    manager.Run(cancellation).Forget();
                    return true;
                }
        }
        return false;
    }

    /// <summary>
    /// サブシーンを非同期ロード
    /// AwakeやStartが呼ばれた状態だが、まだシーンは非表示
    /// </summary>
    /// <param name="name">シーンインデックス</param>
    /// <returns></returns>
    public async UniTask<Scene> LoadSubSceneAsync(SceneIndex.Sub name)
    {
        bool onLoaded = false;
        StartCoroutine(CoLoadScene((int)name, LoadSceneMode.Additive, (x) => onLoaded = x));
        await UniTask.WaitUntil(() => onLoaded == true,
            cancellationToken: gameObject.GetCancellationTokenOnDestroy());

        Debug.Log($"Load {name} scene.");
        return SceneManager.GetSceneByBuildIndex((int)name);
    }

    /// <summary>
    /// サブシーンを非同期アンロード
    /// </summary>
    /// <param name="scene">シーン</param>
    /// <param name="releaseUnusedResouces">未使用リソースをアンロードするか</param>
    /// <returns></returns>
    public async UniTask UnloadSubSceneAsync(Scene scene, bool releaseUnusedResouces)
    {
        var name = scene.name;
        bool onUnloaded = false;
        StartCoroutine(CoUnloadScene(scene, releaseUnusedResouces, (x) => onUnloaded = x));
        await UniTask.WaitUntil(() => onUnloaded == true,
            cancellationToken: gameObject.GetCancellationTokenOnDestroy());

        Debug.Log($"Unload {name} scene.");
    }
    
    IEnumerator CoLoadScene(int buildIndex, LoadSceneMode mode, UnityAction<bool> onLoaded)
    {
        yield return SceneManager.LoadSceneAsync(buildIndex, mode);
        onLoaded?.Invoke(true);
    }

    IEnumerator CoUnloadScene(Scene scene, bool releaseUnusedResouces, UnityAction<bool> onUnloaded)
    {
        yield return SceneManager.UnloadSceneAsync(scene);
        if (releaseUnusedResouces)
            yield return Resources.UnloadUnusedAssets();
        onUnloaded?.Invoke(true);
    }
}
