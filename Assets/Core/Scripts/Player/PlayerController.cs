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

    [SerializeField, Header("クールダウンの時間")]
    float m_coolDownTime = 3;

    [SerializeField, Header("クールダウンになるまでのパリィ時間")]
    float m_timeToCoolDown = 1;

    /// <summary>プレイヤーの最大HP</summary>
    int m_playerMaxHP;
    /// <summary>現在のレベル</summary>
    int m_currentLevel;
    /// <summary>パリィのタイマー</summary>
    float m_parryTimer;
    /// <summary>クールダウンのタイマー</summary>
    float m_coolDownTimer;
    /// <summary>プレイヤーのステート</summary>
    PlayerStateEnum m_state;
    /// <summary>エネミーからの攻撃受付</summary>
    bool m_isHit;
    /// <summary>攻撃待ちのダメージ</summary>
    int m_waitDamage;
    /// <summary>アニメーター</summary>
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
        Parry();
    }

    /// <summary>
    /// Start関数で呼ばれる最初のセットアップ
    /// </summary>
    void SetUp()
    {
        m_anim = GetComponent<Animator>();
        m_playerMaxHP = m_playerHP;
    }

    /// <summary>
    /// 入力処理CoolTimeもここで実装
    /// </summary>
    void PlayerInput()
    {
        if(m_state == PlayerStateEnum.CoolTime)
        {
            m_coolDownTimer += Time.deltaTime;
            if(m_coolDownTimer >= m_coolDownTime)
            {
                m_coolDownTimer = 0;
                StateChange(PlayerStateEnum.Idle);
            }
            else
            {
                return;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            StateChange(PlayerStateEnum.ParryWait);
        }
    }

    void Parry()
    {
        if (m_isHit && m_state == PlayerStateEnum.ParryWait)
        {
            StateChange(PlayerStateEnum.ParrySuccess);
            m_isHit = false;
            m_parryTimer = 0;
            return;
        }

        if (m_state == PlayerStateEnum.ParryWait || m_isHit)
        {
            m_parryTimer += Time.deltaTime;
            if(m_isHit && m_parryTimer > m_changeReception[m_currentLevel] / 2)
            {
                StateChange(PlayerStateEnum.TakeHit);
                m_isHit = false;
                m_parryTimer = 0;
                return;
            }

            if(m_parryTimer > m_timeToCoolDown)
            {
                StateChange(PlayerStateEnum.CoolTime);
                m_isHit = false;
                m_parryTimer = 0;
                return;
            }
        }
    }

    /// <summary>
    /// Stateを切り替える際呼ばれる処理があればここに書く
    /// </summary>
    /// <param name="playerState"></param>
    void StateChange(PlayerStateEnum playerState)
    {
        switch(playerState)
        {
            //通常
            case PlayerStateEnum.Idle:
                m_state = PlayerStateEnum.Idle;
                break;
            //スペース入力した時
            case PlayerStateEnum.ParryWait:
                m_state = PlayerStateEnum.ParryWait;
                m_anim.Play("Parry_Player");
                break;
            //パリィ成功時
            case PlayerStateEnum.ParrySuccess:
                m_state = PlayerStateEnum.ParrySuccess;
                m_anim.Play("Parry_Player");
                break;
            //攻撃受ける時
            case PlayerStateEnum.TakeHit:
                m_state = PlayerStateEnum.TakeHit;
                m_anim.Play("Hit_Player");
                DownHP(m_waitDamage);
                break;
            //クールタイムの時
            case PlayerStateEnum.CoolTime:
                m_state = PlayerStateEnum.CoolTime;
                break;
            //例外
            default:
                Debug.LogError("PlayerStateの例外");
                break;
        }
    }

    /// <summary>
    /// アニメーション戻す
    /// </summary>
    public void HasExitTime()
    {
        m_anim.Play("Idle_Player");
    }

    /// <summary>
    /// 攻撃を受け付ける
    /// </summary>
    public void Hit(int damage)
    {
        m_isHit = true;
        m_waitDamage = damage;
    }

    /// <summary>
    /// プレイヤーの体力を減らす
    /// </summary>
    /// <param name="value">減少値</param>
    void DownHP(int value)
    {
        m_playerHP -= value;
        // HPバー設定
        GameSceneManager.instance.canvasRoot.SetHpFillAmount(m_playerHP / (float)m_playerMaxHP);
        Debug.Log($"現在の体力は{m_playerHP}です");

        if(m_playerHP <= m_playerHP * m_changePlayerHPs[m_currentLevel])
        {
            m_currentLevel++;
        }

        if (m_playerHP <= 0)
        {
            //GameManagerからGameOver呼ぶ
            GameSceneManager.instance.OnGameOver();
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

    /// <summary>
    /// プレイヤーの残りHPパーセンテージを取得
    /// </summary>
    /// <returns>パーセンテージ（0〜100）</returns>
    public int GetPlayerHpPercentage()
    {
        var ratio = m_playerHP / (float)m_playerMaxHP;
        return (int)(ratio * 100);
    }
}
