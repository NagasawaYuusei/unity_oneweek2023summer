using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float m_shakePosx = 1;
    [SerializeField] float m_shakeTime = 0.2f;

    public void Shake()
    {
        StartCoroutine(ShakeMethod());
    }

    IEnumerator ShakeMethod()
    {
        transform.DOMoveX(m_shakePosx, m_shakeTime);
        yield return new WaitForSeconds(m_shakeTime);
        transform.DOMoveX(-m_shakePosx, m_shakeTime);
        yield return new WaitForSeconds(m_shakeTime);
        transform.DOMoveX(0, m_shakeTime);
    }
}
