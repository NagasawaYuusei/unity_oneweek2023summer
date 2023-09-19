using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int Damage;
    private GameObject BattlePosObj;

    //private Transform target;
    private float speed = 5f;
    public enum EnemyState {Idle,Battle,Death}
    protected EnemyState State = EnemyState.Idle;

    private bool once = false;
    void Start()
    {
        BattlePosObj = GameObject.Find("BattlePosition");   
    }

    void Update()
    {
        if (this.transform.position != BattlePosObj.transform.position)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, BattlePosObj.transform.position, speed * Time.deltaTime);
        }
        else
        {
            BattleMode();
        }
    }

 
    public void BattleMode()
    {
        if (once == true) return;
        State = EnemyState.Battle;
    }

    public void Death()
    {
        if (once == true) return;

        StartCoroutine("FadeOutAndDestroy");
        once = true;
    }

    IEnumerator FadeOutAndDestroy()
    {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        for (float t = 1.0f; t > 0; t -= 0.05f)
        {
            renderer.color = new Color(1, 1, 1, t);
            yield return new WaitForSeconds(0.05f);
        }

        Destroy(this.gameObject);
    }
}
