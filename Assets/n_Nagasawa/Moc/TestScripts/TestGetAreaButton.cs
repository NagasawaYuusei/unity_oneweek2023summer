using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGetAreaButton : MonoBehaviour
{
    TestPlayer _testPlayer;
    [SerializeField] float _range;
    void Update()
    {
        if(_testPlayer)
        {

            if (_testPlayer.State == TestPlayer.PlayerState.Button)
            {
                _testPlayer.CatchButton(transform.parent.gameObject);
                _testPlayer = null;
                return;
            }

            if (Vector2.Distance(transform.parent.position, _testPlayer.transform.position) > _range)
            {
                _testPlayer.ChangeState(TestPlayer.PlayerState.None);
                _testPlayer = null;
                return;
            }

            if(_testPlayer.State == TestPlayer.PlayerState.None)
            {
                Debug.LogError("なってほしくないエラー、、、");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out _testPlayer))
        {
            if (_testPlayer.State == TestPlayer.PlayerState.Near)
            {
                _testPlayer = null;
                return;
            }

            _testPlayer.ChangeState(TestPlayer.PlayerState.Near);
            Debug.Log("Player近くにおるで");
        }
    }
}
