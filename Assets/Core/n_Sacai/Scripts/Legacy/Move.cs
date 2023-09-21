using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float speed = 8f;

    private Rigidbody2D rigidbody2d;

    [SerializeField] private float jumpForce = 5.0f;  // ジャンプ力
    private bool isJumping = false; // ジャンプ中かどうか
    private bool canJump = true;    //ジャンプ可能かどうか
    [SerializeField] private float CoolTime = 2f;   //クールタイム

    void Start()
    {
        rigidbody2d = this.GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
        // 入力を取得（-1から1の値）
        float moveHorizontal = Input.GetAxis("Horizontal");

        // Rigidbodyに力を加えて移動
        rigidbody2d.velocity = new Vector2(moveHorizontal * speed, rigidbody2d.velocity.y);

        // ジャンプ処理
        if (Input.GetKey(KeyCode.W) && !isJumping && canJump)
        {
            rigidbody2d.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
            StartCoroutine("JumpCoolTime");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform"))
        {
            isJumping = false;
        }
    }

    IEnumerator JumpCoolTime()
    {
        canJump = false;
        yield return new WaitForSeconds(CoolTime);    //2秒停止
        canJump = true;
    }
}
