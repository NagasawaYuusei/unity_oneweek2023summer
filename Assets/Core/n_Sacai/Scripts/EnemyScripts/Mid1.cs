using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Mid1 : Enemy
{
    [SerializeField] private Status stat;

    private int AttackPower;
    private float speed;

    private GameObject BattleArea;

    private EnemyState Mid1State = EnemyState.Idle;

    private bool check = false;

    private void Start()
    {
        BattleArea = GameObject.Find("BattleArea");
        speed = stat.MoveSpeed;
        AttackPower = stat.AttackPower1;
    }

    private void Update()
    {
        switch (Mid1State)
        {
            case EnemyState.Idle:
                if (this.transform.position == BattleArea.transform.position)
                {
                    Mid1State = EnemyState.Battle;
                }
                base.MoveBattlePos(BattleArea.transform, speed);
                break;

            case EnemyState.Battle:
                StartCoroutine("CountDown");
                Mid1State = EnemyState.Death;
                break;

            case EnemyState.Death:
                if (check == true)
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
