using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy生成クラス
/// EnemyManagerと同じオブジェクトにアタッチします
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    Transform m_InstanceArea; // 生成地点

    [SerializeField]
    EnemyData[] m_enemiesData = null;

    [Serializable]
    class EnemyData
    {
        [SerializeField]
        private GameObject _prefab = null;
        public GameObject prefab => _prefab;

        [SerializeField]
        private Status _status = null;
        public Status status => _status;
    }

    /// <summary>
    /// Core外でつけられた敵の名前（Status ScriptableObject と WavesData ScriptableObject の Name）と
    /// Core内のPrefabを結びつけます
    /// </summary>
    Dictionary<string, EnemyData> m_enemyDict = new Dictionary<string, EnemyData>();

    void Awake()
    {
        foreach (var data in m_enemiesData)
        {
            // 手を抜いて名前の不一致チェックは現状してないです、ちゃんとしたほうがいいです
            m_enemyDict.Add(data.status.Name, data);
        }
    }

    /// <summary>
    /// 敵を生成
    /// </summary>
    /// <param name="name">敵の名前</param>
    /// <returns>生成した敵</returns>
    public Enemy SpawnEnemy(string name)
    {
        if (m_enemyDict.TryGetValue(name, out var data))
        {
            var obj = Instantiate(data.prefab, m_InstanceArea.position, m_InstanceArea.rotation);
            var enemy = obj.GetComponent<Enemy>();
            enemy.SetStatus(data.status);
            return enemy;
        }
        Debug.LogError($"{name}が見つからずに生成できませんでした。");
        return null;
    }
}
