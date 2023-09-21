using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    PlayerStateEnum m_playerStateEnum;
    void ChangeState(PlayerStateEnum state)
    {
        //switch(m_playerStateEnum)
        //{

        //}
    }

    public enum PlayerStateEnum
    {
        Idle,
        ParryWait,
        ParrySuccess,
        TakeHit,
    }
}
