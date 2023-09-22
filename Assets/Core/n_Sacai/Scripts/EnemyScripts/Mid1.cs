using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Mid1 : Enemy
{
    private Animator anim;

    private int AttackPower;
    private float speed;

    private GameObject BattleArea;

    private EnemyState Mid1State = EnemyState.Idle;

    private bool check = false;

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
        switch (Mid1State)
        {
            case EnemyState.Idle:
                if (this.transform.position == BattleArea.transform.position)
                {
                    Mid1State = EnemyState.Anticipation;
                }
                base.MoveBattlePos(BattleArea.transform, speed);
                break;

            case EnemyState.Anticipation:
                anim.SetBool("isIdol", true);
                break;

            case EnemyState.Attack:
                if(!isAttack) Attack();
                anim.SetBool("isAttack", true);
                break;

            case EnemyState.Death:
                if (check)
                {
                    base.Death();
                    check = false;
                }
                break;
        }

    }
    public void AttackStateChange()
    {
        Mid1State = EnemyState.Attack;
    }

    public void DeathStateChange()
    {
        Mid1State = EnemyState.Death;
    }

    public void Attack()
    {
        isAttack = true;
        PlayerController.Instance.Hit(AttackPower);
    }
    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(3f);
        check = true;
    }
}
