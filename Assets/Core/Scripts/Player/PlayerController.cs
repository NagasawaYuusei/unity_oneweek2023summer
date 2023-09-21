using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [SerializeField, Header("Playerの体力")]
    int m_playerHP = 100;

    [SerializeField, Header("Playerのレベル到達HP")]
    float[] m_changePlayerHPs = new float[] {0.9f, 0.6f};

    [SerializeField, Header("Playerの各レベル受付時間")]
    float[] m_changeReception = new float[] {0.4f, 0.8f};

    int m_currentLevel;
    float m_inputTimer;
    PlayerStateEnum m_state;

    Animator m_anim;

    void Awake()
    {
        //シングルトン
        if(Instance && Instance.gameObject)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }

    void Start()
    {
        SetUp();
    }

    void Update()
    {
        PlayerInput();
    }

    void SetUp()
    {
        m_anim = GetComponent<Animator>();
    }

    void PlayerInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_state = PlayerStateEnum.ParryWait;
        }
    }

    void Parry()
    {
        if(m_state == PlayerStateEnum.ParryWait)
        {
            m_inputTimer += Time.deltaTime;
            if(m_inputTimer > m_changeReception[m_currentLevel])
            {

            }
        }
    }

    /// <summary>
    /// 攻撃を受け付ける
    /// </summary>
    public void Hit()
    {

    }

    /// <summary>
    /// プレイヤーの体力を減らす
    /// </summary>
    /// <param name="value">減少値</param>
    public void DownHP(int value)
    {
        m_playerHP -= value;
        Debug.Log($"現在の体力は{m_playerHP}です");
        m_anim.Play("Hit_Player");

        if (m_playerHP <= 0)
        {
            //GameManagerからGameOver呼ぶ
        }
    }

    public enum PlayerStateEnum
    {
        Idle,
        ParryWait,
        ParrySuccess,
        TakeHit,
        CoolTime
    }
}
