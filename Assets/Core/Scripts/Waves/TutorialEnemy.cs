using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : Enemy
{
    [SerializeField]
    GameObject m_deathParticle = null;

    Animator m_animator = null;

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
    }

    /// <summary>
    /// 攻撃予備動作
    /// </summary>
    public void OnAnticipation()
    {
        m_animator.SetBool("isIdol", true);
    }

    /// <summary>
    /// 攻撃動作
    /// </summary>
    public void OnAttack()
    {
        PlayerController.Instance.Hit(0, this, true);
        m_animator.SetBool("isAttack", true);
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void OnDeath()
    {
        Instantiate(m_deathParticle, transform.position, Quaternion.identity, transform);
        Death();
    }
}
