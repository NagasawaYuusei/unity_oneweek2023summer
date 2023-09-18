using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _jumpPower;
    [SerializeField] string _groundTagName = "Respawn";
    Rigidbody2D _rb;
    bool _isJump;
    PlayerState _playerState;
    PlayerGimmick _playerGimmick;
    PlayerGimmick _readyGimmick;
    GameObject _buttonObject;

    public PlayerState State => _playerState;
    public PlayerGimmick Gimmick => _playerGimmick;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();
        Jump();
        Button();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(h * _speed, _rb.velocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && !_isJump)
        {
            _rb.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _isJump = true;
        }
    }

    void Button()
    {
        switch(_playerState)
        {
            case PlayerState.None:
                //なんも持ってない
                break;
            case PlayerState.Near:
                //近くにボタンがある
                if(Input.GetButtonDown("Jump"))
                {
                    ChangeState(PlayerState.Button);
                }
                break;
            case PlayerState.Button:
                if(Input.GetButtonDown("Jump") && _playerGimmick == PlayerGimmick.Near)
                {
                    Debug.Log("呼ばれた");
                    ChangeGimmick(_readyGimmick);
                }
                break;
            default:
                Debug.LogError("起きてたまるか");
                break;
        }
    }

    public void CurrentButton(PlayerGimmick gimmick, GameObject area)
    {
        switch(gimmick)
        {
            case PlayerGimmick.Pocket:
                //
                break;
            case PlayerGimmick.Scaffold:
                _buttonObject.transform.parent = area.transform;
                _buttonObject.transform.position = area.transform.position;
                Collider2D collider = _buttonObject.GetComponent<Collider2D>();
                collider.enabled = true;
                collider.GetComponent<Collider2D>().isTrigger = false;
                break;
            default:
                Debug.LogError("起きてたまるか");
                break;
        }
    }

    public void CatchButton(GameObject button)
    {
        _buttonObject = button;
        button.transform.parent = transform;
        button.transform.position = new Vector2(transform.position.x, transform.position.y + 1.4f);
        button.GetComponent<Collider2D>().enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == _groundTagName)
        {
            _isJump = false;
        }
    }

    public void ChangeState(PlayerState playerState)
    {
        _playerState = playerState;
    }

    public void ChangeGimmick(PlayerGimmick gimmick)
    {
        _playerGimmick = gimmick;
    }

    public void ReadyGimmick(PlayerGimmick gimmick)
    {
        _readyGimmick = gimmick;
    }

    public enum PlayerState
    {
        None,
        Near,
        Button
    }

    public enum PlayerGimmick
    {
        None,
        Near,
        Scaffold,
        Pocket
    }
}
