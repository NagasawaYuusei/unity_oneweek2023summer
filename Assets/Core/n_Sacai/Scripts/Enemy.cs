using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState {Idle, Anticipation, Attack,Death}
    protected EnemyState State = EnemyState.Idle;

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
            c.a -= 0.02f;
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
