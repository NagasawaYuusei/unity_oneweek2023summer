using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Status : ScriptableObject  //これはスクリプタブルオブジェクト
{
    public string Name;
    public int AttackPower1;
    public int AttackPower2;
    public int AttackPower3;
    public float MoveSpeed;
}
