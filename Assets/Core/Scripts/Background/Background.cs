using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using DG.Tweening;

public class Background : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer m_skyRenderer = null;

    public bool isSkyChaging { get; private set; } = false;

    /// <summary>
    /// 空を変更
    /// </summary>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    public async UniTask ChangeSky(Sprite sprite, float duration,
        float delay = 0f, Ease ease = Ease.Linear, CancellationToken cancellation = default)
    {
        if (isSkyChaging)
            return;
        isSkyChaging = true;

        var oldObj = m_skyRenderer.gameObject;
        var newObj = Instantiate(oldObj, oldObj.transform.parent);
        m_skyRenderer = newObj.GetComponent<SpriteRenderer>();
        m_skyRenderer.sprite = sprite;
        m_skyRenderer.color = new Color(1, 1, 1, 0);
        m_skyRenderer.sortingOrder++;
        newObj.name = $"{m_skyRenderer.sortingOrder:D3}_Sky";

        await DOVirtual.Float(0f, 1f, duration,
            (value) =>
            {
                m_skyRenderer.color = new Color(1, 1, 1, value);
            })
            .SetDelay(delay)
            .SetEase(ease)
            .SetUpdate(UpdateType.Normal)
            .WithCancellation(cancellation);

        Destroy(oldObj);
        isSkyChaging = false;
    }
}
