using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Samurai : Enemy
{
    private int AttackPower;
    private float speed;

    private GameObject BattleArea;

    private EnemyState SamuraiState = EnemyState.Idle;

    private bool check = false;

    private void Start()
    {
        BattleArea = GameObject.Find("BattleArea");
        //ここで取得
        speed = GameObject.Find("EnemyManger").GetComponent<EnemyManager>().Small1.MoveSpeed;
        Debug.Log("確認" + speed);
        AttackPower = GameObject.Find("EnemyManager").GetComponent<EnemyManager>().Small1.AttackPower1;
    }

    private void Update()
    {
        //speed = GameObject.Find("EnemyManger").GetComponent<EnemyManager>().Small1.MoveSpeed;
        Debug.Log(speed);

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

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(3f);
        check = true;
    }

}
