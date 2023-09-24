using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private GameObject BAKUSAN;

    private Animator anim;

    private int AttackPower;
    private float speed;

    private GameObject BattleArea;

    int count;

    public enum BossState
    {
        move,
        anticipation,
        attack,
        attackidle,
        idle,
        dbDelay,
        dbAttack,
        lastDelay,
        lastAttack,
        hit,
        death
    };
    private BossState bosstate = BossState.move;

    private bool check = true;

    private void Start()
    {
        transform.position = new Vector2(3f, 0);
        BattleArea = GameObject.Find("BattleArea");
        speed = stat.MoveSpeed;
        AttackPower = stat.GetAttackPower(0);
        anim = this.GetComponent<Animator>();
        StateChange(BossState.move);
    }

    public void StateChange(BossState state)
    {
        if(isParry && count != 2)
        {
            state = BossState.hit;
        }
        switch(state)
        {
            case BossState.move:
                anim.Play("Instantiate_Boss");
                break;
            case BossState.anticipation:
                anim.Play("Delay_Boss");
                break;
            case BossState.attack:
                anim.Play("Attack_Boss");
                break;
            case BossState.attackidle:
                anim.Play("Attack_to_Idle_Boss");
                count++;
                break;
            case BossState.idle:
                anim.Play("Idle_Boss");
                break;
            case BossState.dbDelay:
                anim.Play("DBDelay_Boss");
                break;
            case BossState.dbAttack:
                anim.Play("DBAttack_Boss");
                count++;
                break;
            case BossState.lastDelay:
                anim.Play("SPDelay_Boss");
                break;
            case BossState.lastAttack:
                anim.Play("SPAttack_Boss");
                break;
            case BossState.hit:
                anim.Play("Hit_Boss");
                isParry = false;
                break;
            case BossState.death:
                if (check)
                {
                    Instantiate(BAKUSAN, this.transform.position, Quaternion.identity);
                    bosstate = BossState.death;
                    anim.Play("Down_Boss");
                    base.Death();
                    check = false;
                }
                break;
        }
        bosstate = state;
        Debug.Log($"state : {state}, count : {count}");
    }

    public void HitEnd()
    {
        if(count <= 1)
        {
            StateChange(BossState.dbDelay);
        }
        else if(count == 2)
        {
            StateChange(BossState.lastDelay);
        }
        else
        {
            Debug.LogError("reigai");
        }
    }

    public void Attack(HitAnim anim)
    {
        bool on = false;
        if (anim == HitAnim.on)
        {
            on = true;
        }
        PlayerController.Instance.Hit(AttackPower, this, on);
    }
}
