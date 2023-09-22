using UnityEngine;

[CreateAssetMenu]
public class Status : ScriptableObject  //これはスクリプタブルオブジェクト
{
    [SerializeField]
    private string _Name;

    [SerializeField]
    private float _MoveSpeed;

    [SerializeField]
    private int[] _AttackPower;

    /// <summary>
    /// 移動速度
    /// </summary>
    public float MoveSpeed => _MoveSpeed;

    /// <summary>
    /// 攻撃力
    /// </summary>
    public int GetAttackPower(int index)
    {
        if (_AttackPower == null)
        {
            Debug.LogError($"{_Name}のAttackPowerがNullです！");
            return 0;
        }
        if ((index < 0) || (_AttackPower.Length <= index))
        {
            Debug.LogError($"{_Name}のAttackPowerに無効なインデックス{index}が指定されました。");
            return 0;
        }
        return _AttackPower[index];
    }
}
