using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//各敵が継承している親クラス
public class Enemy : MonoBehaviour
{
    public enum EnemyState {Idle, Anticipation, Attack,Death}       //ボス以外の敵のステート管理enum
    protected EnemyState State = EnemyState.Idle;

    //バトルエリアまで移動する関数
    protected virtual void MoveBattlePos(Transform BattlePos,float speed)
    {
        this.transform.position = Vector3.MoveTowards(transform.position, BattlePos.position, speed * Time.deltaTime);
    }

    //死亡時に呼ぶとフェードアウトコルーチンをスタートさせる
    protected void Death()
    {
        StartCoroutine("FadeOutAndDestroy");
    }

    //フェードアウトコルーチン
    IEnumerator FadeOutAndDestroy()
    {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        Color c = this.GetComponent<SpriteRenderer>().color;
        c.a = 1f;
        while (true)
        {
            yield return new WaitForSeconds(0.01f); //フェードアウト速度
            c.a -= 0.005f;                          //アルファ値減算

            renderer.color = new Color(c.r,c.g,c.b,c.a);
            if(c.a <= 0f)
            {
                c.a = 1f;
                Destroy(this.gameObject);
                break;
            }
        }
    }
}
