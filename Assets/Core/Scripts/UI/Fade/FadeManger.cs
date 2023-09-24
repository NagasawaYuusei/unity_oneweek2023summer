using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using KyawaLib;
using UnityEngine;

public class FadeManger : SingletonMonoBehaviour<FadeManger>
{
    [SerializeField]
    GameObject m_faderPrefab = null;

    Fader m_fader = null;
    public Fader fader => m_fader;

    [SerializeField]
    FadeDataScrObj m_titleFadeData = null;
    [SerializeField]
    FadeDataScrObj m_gameFadeData = null;
    [SerializeField]
    FadeDataScrObj m_resultFadeData = null;
    [SerializeField]
    FadeDataScrObj m_tutorialFadeData = null;

    Dictionary<Fade.Situation, FadeDataScrObj> m_dict = null;

    /// <summary>
    /// 初期化　必ず最初に呼び出します.
    /// </summary>
    public void Initialize()
    {
        m_fader = Instantiate(m_faderPrefab, transform).GetComponent<Fader>();

        m_dict = new Dictionary<Fade.Situation, FadeDataScrObj>
            {
                { global::Fade.Situation.Title, m_titleFadeData },
                { global::Fade.Situation.Game, m_gameFadeData },
                { global::Fade.Situation.Result, m_resultFadeData },
                { global::Fade.Situation.Tutorial, m_tutorialFadeData },
            };
    }

    /// <summary>
    /// フェード（FadeData参照）
    /// </summary>
    /// <param name="situation">シーン遷移状況</param>
    /// <param name="type">フェードイン/アウト</param>
    /// <returns></returns>
    public async UniTask Fade(Fade.Situation situation, Fade.Type type)
    {
        FadeDataScrObj fadeDataObj;
        if (m_dict.TryGetValue(situation, out fadeDataObj))
        {
            if (fadeDataObj == null)
            {
                Debug.LogError($"{situation}のフェードデータがNullです。");
                return;
            }
        }
        else
        {
            Debug.LogError($"{situation}のフェードデータが見つかりませんでした。");
            return;
        }

        if (type == global::Fade.Type.FadeIn)
        {
            var data = fadeDataObj.fadeInData;
            await m_fader.FadeIn(data.duration, data.color, data.delay, data.ease);
        }
        else
        {
            var data = fadeDataObj.fadeOutData;
            await m_fader.FadeOut(data.duration, data.color, data.delay, data.ease);
        }
    }
}
