using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//ここもSamuraiコピペ
//やることはZako2と同じ
public class Mid1 : Enemy
{
    [SerializeField] private GameObject BAKUSAN;

    private Animator anim;

    private int AttackPower;
    private float speed;

    private GameObject BattleArea;

    private EnemyState Mid1State = EnemyState.Idle;

    private bool check = true;

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
                anim.SetBool("isAttack", true);
                break;

            case EnemyState.Death:
                if (check)
                {
                    if (isParry)
                    {
                        anim.Play("Hit_Nata");
                    }
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
        Instantiate(BAKUSAN, this.transform.position, Quaternion.identity);
        Mid1State = EnemyState.Death;
    }

    public void Attack(HitAnim hit)
    {
        bool on = false;
        if(hit == HitAnim.on)
        {
            on = true;
        }
        PlayerController.Instance.Hit(AttackPower, this, on);
    }
}
