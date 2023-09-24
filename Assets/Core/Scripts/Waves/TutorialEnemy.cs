using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : Enemy
{
    [SerializeField]
    GameObject m_deathParticle = null;

    Animator m_animator = null;
    bool m_onDeath = false; // パリィが失敗したら死なない、成功したときにtrue担って死ぬ

    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 初期状態
    /// </summary>
    public void Reset()
    {
        m_animator.SetTrigger("idle");
        m_animator.SetBool("isIdol", false);
        m_animator.SetBool("isAttack", false);
        m_onDeath = false;
    }

    /// <summary>
    /// 攻撃予備動作
    /// </summary>
    public void OnAnticipation()
    {
        m_animator.SetBool("isIdol", true);
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void OnDeath()
    {
        m_onDeath = true;
    }

    // 以下、Animationからの呼び出し

    public void AttackStateChange()
    {
        m_animator.SetBool("isAttack", true);
    }
    public void Attack()
    {
        PlayerController.Instance.Hit(0, this, true);
    }
    public void DeathStateChange()
    {
        if (m_onDeath)
        {
            Instantiate(m_deathParticle, transform.position, Quaternion.identity, transform);
            Death();
        }
    }
}
