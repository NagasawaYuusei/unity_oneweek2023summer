using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//一発のみの攻撃を行うザコ敵1
public class Samurai : Enemy
{
    [SerializeField] private Status stat;           //ScriptableObjectを入れて
    [SerializeField] private GameObject BAKUSAN;    //敵死亡時パーティクル

    private Animator anim;

    private int AttackPower;                        //攻撃力
    private float speed;                            //生成後バトルエリアまで移動してくる速度

    private GameObject BattleArea;                  //バトルポジション

    private EnemyState SamuraiState = EnemyState.Idle;      //ザコ敵ステート

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
        switch (SamuraiState)
        {
            case EnemyState.Idle:   //アイドル状態時は移動
                if (this.transform.position == BattleArea.transform.position)
                {
                    SamuraiState = EnemyState.Anticipation;     //バトルエリアについたら即時予備動作に遷移
                }
                base.MoveBattlePos(BattleArea.transform,speed);
                break;

            case EnemyState.Anticipation:
                anim.SetBool("isIdol", true);       //予備動作アニメーションを再生
                break;

            case EnemyState.Attack:
                if(!isAttack) Attack();             //アタック状態ならプレイヤー側に伝えるAttack関数を一度だけ呼び出し
                anim.SetBool("isAttack", true);
                break;

            case EnemyState.Death:
                if (check)              //フェードアウト関数が連続で呼ばれるのを防ぐbool変数
                {
                    base.Death();       //フェードアウト
                    check = false;
                }
                break;
        }
    }

    //予備動作アニメーション終了時にAddEventで呼び出される
    public void AttackStateChange()
    {
        SamuraiState = EnemyState.Attack;
    }

    //攻撃アニメーション終了時にAddEventで呼び出される
    public void DeathStateChange()
    {
        Instantiate(BAKUSAN, this.transform.position, Quaternion.identity, this.transform);     //自分の配下にパーティクル生成
        SamuraiState = EnemyState.Death;
    }

    //プレイヤー側に攻撃を伝える
    public void Attack()
    {
        isAttack = true;
        PlayerController.Instance.Hit(AttackPower);
    }
}
