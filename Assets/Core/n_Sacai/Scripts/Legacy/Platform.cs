using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    bool setOff;
    BoxCollider2D colliderOfGround;

    void Start()
    {
        colliderOfGround = this.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (setOff)
        {
            colliderOfGround.enabled = false;
        }
        if (!setOff)
        {
            colliderOfGround.enabled = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            setOff = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            setOff = false;
        }
    }
}
