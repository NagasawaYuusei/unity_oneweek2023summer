using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState {Idle, Anticipation, Attack,Death}
    public enum HitAnim { off, on}
    protected EnemyState State = EnemyState.Idle;

    /// <summary>
    /// 死んでいるか
    /// </summary>
    public bool isDead => (State == EnemyState.Death);

    /// <summary>
    /// ステータス
    /// </summary>
    protected Status stat;

    protected bool isParry;

    /// <summary>
    /// ステータスを設定（最初に呼び出します）
    /// </summary>
    /// <param name="status"></param>
    public void SetStatus(Status status)
    {
        stat = status;
    }

    public void Collect()
    {
        isParry = true;
    }
        
    public void AnimationPlaySE(SoundType.SE se)
    {
        AudioManager.Instance.PlaySE(se);
    }

    protected virtual void MoveBattlePos(Transform BattlePos,float speed)
    {
        this.transform.position = Vector3.MoveTowards(transform.position, BattlePos.position, speed * Time.deltaTime);
    }

    protected void BattleMode()
    {
        State = EnemyState.Anticipation;
    }

    protected void Death()
    {
        StartCoroutine("FadeOutAndDestroy");
    }

    IEnumerator FadeOutAndDestroy()
    {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        Color c = this.GetComponent<SpriteRenderer>().color;
        c.a = 1f;
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            c.a -= 0.005f;
            renderer.color = new Color(c.r,c.g,c.b,c.a);
            if(c.a <= 0f)
            {
                c.a = 1f;
                /* Kawata:
                 *   Enemy側でGameObjectをDestroyしたままEnemyManagerの保持しているEnemy変数のnull判定してましたが、
                 *   Destroyした後の変数にはちゃんとnullを入れて上げたほうがいいです。
                 *   EnemyManager側でDestroyしてnullを入れるようにしました。
                 */
                State = EnemyState.Death;
                //Destroy(this.gameObject);
                break;
            }
        }
    }
}
