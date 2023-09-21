using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace KyawaLib
{
    public class Fader : MonoBehaviour
    {
        /// <summary>
        /// UI参照
        /// </summary>
        FaderCanvasRoot m_canvasRoot = null;

        /// <summary>
        /// フェード処理キャンセル用
        /// </summary>
        CancellationTokenSource m_fadeCts = null;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns></returns>
        void Awake()
        {
            m_canvasRoot = gameObject.GetComponentInChildren<FaderCanvasRoot>();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        void OnDestroy()
        {
            m_fadeCts?.Cancel();
            m_canvasRoot = null;
        }

        /// <summary>
        /// フェード処理をキャンセル
        /// 完全に終了するまで待機できます。
        /// </summary>
        /// <returns>フェード処理キャンセル中にゲームオブジェクトが削除されるとキャンセルされてtrueを返します。</returns>
        async UniTask<bool> CancelFade()
        {
            if (m_fadeCts == null)
                return false;

            m_fadeCts.Cancel();
            var token = gameObject.GetCancellationTokenOnDestroy();
            await UniTask.WaitUntil(() => m_fadeCts == null, cancellationToken: token);
            return token.IsCancellationRequested;
        }

        /// <summary>
        /// フェード処理
        /// </summary>
        /// <param name="from">初期値</param>
        /// <param name="to">最終値</param>
        /// <param name="duration">所要時間(s)</param>
        /// <param name="color">フェードカラー(RGB)</param>
        /// <param name="delay">遅延時間(s)</param>
        /// <param name="ease">イージング</param>
        /// <returns></returns>
        async UniTask Fade(float from, float to, float duration, Color? color,
            float delay = 0f,
            Ease ease = Ease.Linear)
        {
            var image = m_canvasRoot?.Image;
            if (image == null)
                return;

            var col = Color.black;
            if (color != null)
                col = (Color)color;

            if (duration <= 0f)
            {
                col.a = to;
                image.color = col;
                return;
            }

            col.a = from;
            image.color = col;

            m_fadeCts = new CancellationTokenSource();

            await DOVirtual.Float(from, to, duration,
                value =>
                {
                    if (image)
                    {
                        col.a = value;
                        image.color = col;
                    }
                })
                .SetDelay(delay)
                .SetEase(ease)
                .SetUpdate(UpdateType.Normal)
                .WithCancellation(m_fadeCts.Token);

            m_fadeCts.Dispose();
            m_fadeCts = null;
        }

        /// <summary>
        /// フェードイン
        /// </summary>
        /// <param name="duration">所要時間(s)</param>
        /// <param name="color">フェードカラー(RGB)</param>
        /// <param name="delay">遅延時間(s)</param>
        /// <param name="ease">イージング</param>
        /// <returns></returns>
        public async UniTask FadeIn(float duration,
            Color? color = null,
            float delay = 0f,
            Ease ease = Ease.Linear)
        {
            var isCanceled = await CancelFade();
            if (isCanceled)
                return;

            await Fade(1f, 0f, duration, color, delay, ease);
            m_canvasRoot.Image.raycastTarget = false;
        }

        /// <summary>
        /// フェードアウト
        /// </summary>
        /// <param name="duration">所要時間(s)</param>
        /// <param name="color">フェードカラー(RGB)</param>
        /// <param name="delay">遅延時間(s)</param>
        /// <param name="ease">イージング</param>
        /// <returns></returns>
        public async UniTask FadeOut(float duration,
            Color? color = null,
            float delay = 0f,
            Ease ease = Ease.Linear)
        {
            var isCanceled = await CancelFade();
            if (isCanceled)
                return;

            m_canvasRoot.Image.raycastTarget = true;
            await Fade(0f, 1f, duration, color, delay, ease);
        }
    }
}