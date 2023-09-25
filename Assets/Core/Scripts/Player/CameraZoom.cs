using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    CameraZoomPreset[] _preset;

    [Serializable]
    class CameraZoomPreset
    {
        [SerializeField]
        private Vector2 _position;
        public Vector2 position => _position;

        [SerializeField]
        private float _size;
        public float size => _size;

        [SerializeField]
        private float _duration;
        public float duration => _duration;

        [SerializeField]
        private Ease _ease = Ease.Linear;
        public Ease ease => _ease;
    }

    Camera m_camera = null;

    void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    async UniTask SetCameraPosition(Vector3 pos, float duration, Ease ease,
        CancellationToken cancellation = default)
    {
        await transform.DOMove(pos, duration)
            .SetEase(ease)
            .SetUpdate(UpdateType.Normal)
            .WithCancellation(cancellation);
    }

    async UniTask SetCameraSize(float from, float to,float duration, Ease ease,
        CancellationToken cancellation = default)
    {
        await DOVirtual.Float(from, to, duration,
            (value) =>
            {
                if (m_camera)
                    m_camera.orthographicSize = value;
            })
            .SetEase(ease)
            .SetUpdate(UpdateType.Normal)
            .WithCancellation(cancellation);
    }

    /// <summary>
    /// カメラを指定位置・倍率へ更新
    /// </summary>
    /// <param name="index"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    public async UniTask UpdateCamera(int index,
        CancellationToken cancellation = default)
    {
        var data = _preset[index];
        var duration = data.duration;
        var ease = data.ease;
        // 移動
        var pos = new Vector3(data.position.x, data.position.y, m_camera.transform.position.z);
        var posTask = SetCameraPosition(pos, duration, ease, cancellation);
        // ズーム倍率
        var from = m_camera.orthographicSize;
        var to = data.size;
        var zoomTask = SetCameraSize(from, to, duration, ease, cancellation);
        // 両方実行
        await UniTask.WhenAll(posTask, zoomTask);
    }
}
