using UnityEngine;
using System;

/// <summary>
/// ゲームの管理クラス
/// </summary>
public class GameManager
{
    #region プロパティ
    /// <summary>
    /// GameManagerのインスタンス
    /// </summary>
    public static GameManager Instance = new GameManager();
    #endregion

    #region 変数
    GameState _gameState;
    bool _isPause;
    #endregion

    //コンストラクタ
    public GameManager()
    {
        Debug.Log("New GameManager");
    }

    #region イベント

    /// <summary>
    /// ポーズ時の処理を登録
    /// </summary>
    public event Action OnPause;
    /// <summary>
    /// ポーズ解除時の処理を登録
    /// </summary>
    public event Action OnResume;
    /// <summary>
    /// ゲームオーバー時のイベント
    /// </summary>
    public event Action OnGameOverEvent;
    /// <summary>
    /// ゲーム終了時のイベント
    /// </summary>
    public event Action OnGameEndEvent;

    #endregion
    /// <summary>
    /// ゲームの遷移
    /// </summary>
    /// <param name="mode"></param>
    public void GameStateChange(GameState mode)
    {
        if (_gameState == mode)
        {
            Debug.Log("GameStateが一緒です");
            return;
        }

        switch (mode)
        {
            case GameState.GameReady:
                break;
            case GameState.InGame:
                break;
            case GameState.GameFinish:
                break;
        }

        _gameState = mode;
        Debug.Log($"モードを切り替えた {mode}");
    }

    /// <summary>
    /// 変数の初期設定
    /// </summary>
    /// <param name="attachment"></param>
    public void OnSetup(GameManagerAttachment attachment)
    {
        GameStateChange(GameState.GameReady);
    }

    /// <summary>
    /// ゲームオーバー時に呼ぶ
    /// </summary>
    public void OnGameOver()
    {
        OnGameOverEvent?.Invoke();
        GameStateChange(GameState.GameFinish);
        Debug.Log("OnGameOver");
    }

    /// <summary>
    /// ゲームクリア時に呼ぶ
    /// </summary>
    public void OnGameEnd()
    {
        OnGameEndEvent?.Invoke();
        GameStateChange(GameState.GameFinish);
        Debug.Log("OnGameClear");
    }

    /// <summary>
    /// アップデートで呼ぶ
    /// </summary>
    void OnUpdate()
    {
        if (_gameState == GameState.GameFinish) return;

        if (_gameState == GameState.GameReady)
        {
            
        }

        if (Input.GetButtonDown("ポーズキー"))
        {
            if (_gameState != GameState.InGame) return;

            if (_isPause)
            {
                OnResume?.Invoke();
                Cursor.lockState = CursorLockMode.Locked;
                Debug.Log("ポーズ解除");
            }
            else
            {
                OnPause?.Invoke();
                Cursor.lockState = CursorLockMode.None;
                Debug.Log("ポーズ開始");
            }

            _isPause = !_isPause;
        }
    }

    #region コールバック

    public void SetupUpdateCallback(GameManagerAttachment attachment)
    {
        attachment.SetupCallBack(OnUpdate);
    }

    #endregion
}

public enum GameState
{
    GameReady = 0,
    InGame = 1,
    GameFinish = 2,
}