using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Mid2 : Enemy
{
    private Animator anim;

    private int AttackPower;
    private float speed;

    private GameObject BattleArea;

    private EnemyState Mid2State = EnemyState.Idle;

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
        switch (Mid2State)
        {
            case EnemyState.Idle:
                if (this.transform.position == BattleArea.transform.position)
                {
                    Mid2State = EnemyState.Anticipation;
                }
                base.MoveBattlePos(BattleArea.transform, speed);
                break;

            case EnemyState.Anticipation:
                anim.SetBool("isIdol", true);
                break;

            case EnemyState.Attack:
                if (!isAttack) Attack();
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
        Mid2State = EnemyState.Attack;
    }

    public void DeathStateChange()
    {
        Mid2State = EnemyState.Death;
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
