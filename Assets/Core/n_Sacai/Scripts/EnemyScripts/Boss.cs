using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボスクラス
//現状ボスは雑魚敵たちと同じ位置にBattleAreaを指定しているため変える必要があるかも
//EnemyManagerのBattleAreaとINstaceAreaを調節してください
public class Boss : Enemy
{
    //ここから

    [SerializeField] private Status stat;
    private Animator anim;

    private int AttackPower;
    private float speed;

    private GameObject BattleArea;

    //ここまでの変数はSamuraiと同じ役割(時間なくて一個ずつコメント書けませんでした。すいません)

    public enum BossState {move,attackidle,idle,anticipation,attack,death};     //ボス専用ステート
    private BossState bosstate = BossState.move;

    private bool check = true;

    private bool isAttack;

    private void Start()
    {
        BattleArea = GameObject.Find("BattleArea");
        speed = stat.MoveSpeed;
        AttackPower = stat.AttackPower1;
        anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        switch (bosstate)
        {
            //現状は1ループのみ
            //switch文遷移順。move→anticipation→attack→attackidle→idle→death
            
            //生成後バトルエリアまで移動
            case BossState.move:
                if (this.transform.position == BattleArea.transform.position)
                {
                    bosstate = BossState.anticipation;
                }
                base.MoveBattlePos(BattleArea.transform, speed);
                break;

            //予備動作を再生
            case BossState.anticipation:
                anim.SetBool("isDelay", true);
                break;

            //攻撃関数及び攻撃アニメーション再生
            case BossState.attack:
                if(!isAttack) Attack();
                anim.SetBool("isAttack", true);
                break;

            //攻撃アイドルアニメーション再生
            case BossState.attackidle:
                anim.SetBool("isAttackIdle", true);
                break;

            //アイドル状態
            case BossState.idle:
                anim.SetBool("isIdol", true);
                break;

            //死亡
            case BossState.death:
                if (check)
                {
                    base.Death();
                    check = false;
                }
                break;
        }
    }

    //ここから

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

    //ここまでの関数は各Add Eventでアニメーション再生終了時に呼び出し

    public void Attack()
    {
        isAttack = true;
        PlayerController.Instance.Hit(AttackPower);
    }
}
