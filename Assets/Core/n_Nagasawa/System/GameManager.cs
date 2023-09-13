using UnityEngine;
using System;

/// <summary>
/// �Q�[���̊Ǘ��N���X
/// </summary>
public class GameManager
{
    #region �v���p�e�B
    /// <summary>
    /// GameManager�̃C���X�^���X
    /// </summary>
    public static GameManager Instance = new GameManager();
    #endregion

    #region �ϐ�
    GameState _gameState;
    bool _isPause;
    #endregion

    //�R���X�g���N�^
    public GameManager()
    {
        Debug.Log("New GameManager");
    }

    #region �C�x���g

    /// <summary>
    /// �|�[�Y���̏�����o�^
    /// </summary>
    public event Action OnPause;
    /// <summary>
    /// �|�[�Y�������̏�����o�^
    /// </summary>
    public event Action OnResume;
    /// <summary>
    /// �Q�[���I�[�o�[���̃C�x���g
    /// </summary>
    public event Action OnGameOverEvent;
    /// <summary>
    /// �Q�[���I�����̃C�x���g
    /// </summary>
    public event Action OnGameEndEvent;

    #endregion
    /// <summary>
    /// �Q�[���̑J��
    /// </summary>
    /// <param name="mode"></param>
    public void GameStateChange(GameState mode)
    {
        if (_gameState == mode)
        {
            Debug.Log("GameState���ꏏ�ł�");
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
        Debug.Log($"���[�h��؂�ւ��� {mode}");
    }

    /// <summary>
    /// �ϐ��̏����ݒ�
    /// </summary>
    /// <param name="attachment"></param>
    public void OnSetup(GameManagerAttachment attachment)
    {
        GameStateChange(GameState.GameReady);
    }

    /// <summary>
    /// �Q�[���I�[�o�[���ɌĂ�
    /// </summary>
    public void OnGameOver()
    {
        OnGameOverEvent?.Invoke();
        GameStateChange(GameState.GameFinish);
        Debug.Log("OnGameOver");
    }

    /// <summary>
    /// �Q�[���N���A���ɌĂ�
    /// </summary>
    public void OnGameEnd()
    {
        OnGameEndEvent?.Invoke();
        GameStateChange(GameState.GameFinish);
        Debug.Log("OnGameClear");
    }

    /// <summary>
    /// �A�b�v�f�[�g�ŌĂ�
    /// </summary>
    void OnUpdate()
    {
        if (_gameState == GameState.GameFinish) return;

        if (_gameState == GameState.GameReady)
        {
            
        }

        if (Input.GetButtonDown("�|�[�Y�L�["))
        {
            if (_gameState != GameState.InGame) return;

            if (_isPause)
            {
                OnResume?.Invoke();
                Cursor.lockState = CursorLockMode.Locked;
                Debug.Log("�|�[�Y����");
            }
            else
            {
                OnPause?.Invoke();
                Cursor.lockState = CursorLockMode.None;
                Debug.Log("�|�[�Y�J�n");
            }

            _isPause = !_isPause;
        }
    }

    #region �R�[���o�b�N

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