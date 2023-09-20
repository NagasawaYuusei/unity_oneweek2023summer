using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Samurai : Enemy
{
    [SerializeField] private Status stat;

    private int AttackPower;
    private float speed;

    private GameObject BattleArea;

    private EnemyState SamuraiState = EnemyState.Idle;

    private bool check = false;

    private void Start()
    {
        BattleArea = GameObject.Find("BattleArea");
        speed = stat.MoveSpeed;
        AttackPower = stat.AttackPower1;
    }

    private void Update()
    {
        switch (SamuraiState)
        {
            case EnemyState.Idle:
                if (this.transform.position == BattleArea.transform.position)
                {
                    SamuraiState = EnemyState.Battle;
                }
                base.MoveBattlePos(BattleArea.transform,speed);
                break;

            case EnemyState.Battle:
                StartCoroutine("CountDown");
                SamuraiState = EnemyState.Death;
                break;

            case EnemyState.Death:
                if(check == true)
                {
                    base.Death();
                    check = false;
                }   
                break;
        }
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
