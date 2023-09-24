using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Samurai : Enemy
{
    [SerializeField] private GameObject BAKUSAN;

    private Animator anim;

    private int AttackPower;
    private float speed;

    private GameObject BattleArea;

    private EnemyState SamuraiState = EnemyState.Idle;

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
        switch (SamuraiState)
        {
            case EnemyState.Idle:
                if (this.transform.position == BattleArea.transform.position)
                {
                    SamuraiState = EnemyState.Anticipation;
                }
                base.MoveBattlePos(BattleArea.transform,speed);
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
                    if(isParry)
                    {
                        anim.Play("Hit_Zako");
                    }
                    base.Death();
                    check = false;
                }
                break;
        }
    }

    public void AttackStateChange()
    {
        SamuraiState = EnemyState.Attack;
    }

    public void DeathStateChange()
    {
        Instantiate(BAKUSAN, this.transform.position, Quaternion.identity, this.transform);
        SamuraiState = EnemyState.Death;
    }

    public void Attack()
    {
        PlayerController.Instance.Hit(AttackPower, this, true);
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("isAttack", true);
        check = true;
    }

}
