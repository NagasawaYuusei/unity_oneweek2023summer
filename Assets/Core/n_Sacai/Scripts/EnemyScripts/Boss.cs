using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private Animator anim;

    private int AttackPower;
    private float speed;

    private GameObject BattleArea;

    public enum BossState {move,attackidle,idle,anticipation,attack,death};
    private BossState bosstate = BossState.move;

    private bool check = true;

    private bool isAttack;

    private void Start()
    {
        BattleArea = GameObject.Find("BattleArea");
        speed = stat.MoveSpeed;
        AttackPower = stat.GetAttackPower(0);
        anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        switch (bosstate)
        {
            case BossState.move:
                if (this.transform.position == BattleArea.transform.position)
                {
                    bosstate = BossState.anticipation;
                }
                base.MoveBattlePos(BattleArea.transform, speed);
                break;

            case BossState.anticipation:
                anim.SetBool("isDelay", true);
                break;

            case BossState.attack:
                //if(!isAttack) Attack();
                anim.SetBool("isAttack", true);
                break;

            case BossState.attackidle:
                anim.SetBool("isAttackIdle", true);
                break;

            case BossState.idle:
                anim.SetBool("isIdol", true);
                break;

            case BossState.death:
                if (check)
                {
                    base.Death();
                    check = false;
                }
                break;
        }
    }

    public void IdolStateChange()
    {
        bosstate = BossState.death;
        anim.SetBool("isAttackIdle", false);
    }

    public void AttackIdolStateChange()
    {
        bosstate = BossState.attackidle;
        anim.SetBool("isAttack", false);
    }

    public void AttackStateChange()
    {
        bosstate = BossState.attack;
        anim.SetBool("isDelay", false);
    }

    public void DeathStateChange()
    {
        bosstate = BossState.death;
    }

    public void Attack(bool on)
    {
        isAttack = true;
        PlayerController.Instance.Hit(AttackPower, this, on);
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("isAttack", true);
        check = true;
    }

}
