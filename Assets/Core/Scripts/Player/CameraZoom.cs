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

    public async UniTask SetCameraPosition(int index,
        CancellationToken cancellation = default)
    {
        var data = _preset[index];

        var targetPos = new Vector3(data.position.x, data.position.y, m_camera.transform.position.z);
        await transform.DOMove(targetPos, data.duration)
            .SetEase(data.ease)
            .SetUpdate(UpdateType.Normal)
            .WithCancellation(cancellation);
    }

    public async UniTask SetCameraSize(int index,
    CancellationToken cancellation = default)
    {
        var data = _preset[index];

        var startSize = m_camera.orthographicSize;
        await DOVirtual.Float(startSize, data.size, data.duration,
            (value) =>
            {
                if (m_camera)
                    m_camera.orthographicSize = value;
            })
            .SetEase(data.ease)
            .SetUpdate(UpdateType.Normal)
            .WithCancellation(cancellation);
    }
}
