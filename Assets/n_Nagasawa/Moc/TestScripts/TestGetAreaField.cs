using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGetAreaField : MonoBehaviour
{
    TestPlayer _testPlayer;
    [SerializeField] TestPlayer.PlayerGimmick _gimmick;
    [SerializeField] float _range;
    void Update()
    {
        if (_testPlayer)
        {
            Debug.Log(Vector2.Distance(transform.position, _testPlayer.transform.position));
            if (Vector2.Distance(transform.parent.position, _testPlayer.transform.position) > _range)
            {
                
                _testPlayer.ChangeGimmick(TestPlayer.PlayerGimmick.None);
                _testPlayer = null;
                return;
            }

            if (_testPlayer.Gimmick == _gimmick)
            {
                _testPlayer.CurrentButton(_gimmick, gameObject);
                _testPlayer.ChangeState(TestPlayer.PlayerState.None);
                _testPlayer.ChangeGimmick(TestPlayer.PlayerGimmick.None);
                _testPlayer = null;
                return;
            }

            if (_testPlayer.Gimmick == TestPlayer.PlayerGimmick.None)
            {
                Debug.LogError("なってほしくないエラー、、、");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out _testPlayer))
        {
            if (_testPlayer.State != TestPlayer.PlayerState.Button)
            {
                _testPlayer = null;
                return;
            }

            _testPlayer.ChangeGimmick(TestPlayer.PlayerGimmick.Near);
            _testPlayer.ReadyGimmick(_gimmick);
            Debug.Log("Player近くにおるで");
        }
    }
}
