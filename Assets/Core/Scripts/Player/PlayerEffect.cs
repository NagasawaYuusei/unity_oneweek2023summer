using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    /// <summary>アニメーター</summary>
    Animator m_effectAnim;

    void Start()
    {
        m_effectAnim = GetComponent<Animator>();
    }

    /// <summary>
    /// アニメーション戻す
    /// </summary>
    public void EffectHasExitTime()
    {
        m_effectAnim.Play("Idle_PlayerEffect");
    }
}
