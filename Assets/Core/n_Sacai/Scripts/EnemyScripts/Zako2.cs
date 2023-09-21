using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Zako2 : Enemy
{
    [SerializeField] private Status stat;
    private Animator anim;

    private int AttackPower;
    private float speed;


    private GameObject BattleArea;

    private EnemyState Zako2State = EnemyState.Idle;

    private bool check = false;

    private void Start()
    {
        BattleArea = GameObject.Find("BattleArea");
        speed = stat.MoveSpeed;
        AttackPower = stat.AttackPower1;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (Zako2State)
        {
            case EnemyState.Idle:
                if (this.transform.position == BattleArea.transform.position)
                {
                    Zako2State = EnemyState.Anticipation;
                }
                base.MoveBattlePos(BattleArea.transform, speed);
                break;

            case EnemyState.Anticipation:
                anim.SetBool("isIdol", true);
                break;

            case EnemyState.Attack:
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
        Zako2State = EnemyState.Attack;
    }

    public void DeathStateChange()
    {
        Zako2State = EnemyState.Death;
    }

    public void Attack()
    {

    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(3f);
        check = true;
    }

}
