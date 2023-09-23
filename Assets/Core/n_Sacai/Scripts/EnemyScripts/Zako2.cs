using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//現状Samuraiのコピペです
//多段攻撃を行う場合、statからAttackPower2を参照してAttack関数に渡してください。(2回目以降の攻撃力が同じなら必要ない）
//どういうアニメションか分からないのでなんとも言えないですが、一度アイドル状態を挟まずに攻撃が連続でくる場合2回目以降の攻撃タイミングでAdd Eventを呼び出すだけ
//もしアイドル状態を一度挟むアニメーションの場合、何とかしてください！！

public class Zako2 : Enemy
{
    private Animator anim;

    private int AttackPower;
    private float speed;


    private GameObject BattleArea;

    private EnemyState Zako2State = EnemyState.Idle;

    private bool check = false;

    private bool isAttack;

    private void Start()
    {
        BattleArea = GameObject.Find("BattleArea");
        speed = stat.MoveSpeed;
        AttackPower = stat.GetAttackPower(0);
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
        Zako2State = EnemyState.Attack;
    }

    public void DeathStateChange()
    {
        Zako2State = EnemyState.Death;
    }

    public void Attack()
    {
        isAttack = true;
        PlayerController.Instance.Hit(AttackPower);
    }

}
