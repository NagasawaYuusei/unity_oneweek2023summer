using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Data;

public class EnemyManager : KyawaLib.SingletonMonoBehaviour<EnemyManager>
{
    /// <summary>
    /// Waveデータ
    /// </summary>
    [SerializeField]
    private WavesDataScrObj m_wavesData = null;

    /// <summary>
    /// ボス前のWave数
    /// </summary>
    public int wavesCount => m_wavesData.waveData.Length;

    /// <summary>
    /// Enemyスポナー
    /// </summary>
    private EnemySpawner m_enemySpawner = null;

    private bool m_isWorking = false;
    /// <summary>
    /// スポナー稼働中か
    /// </summary>
    public bool isWorking => m_isWorking;

    /// <summary>
    /// 現在プレイヤーと戦っている敵
    /// </summary>
    private Enemy m_enemy = null;

    private void Start()
    {
        m_enemySpawner = GetComponent<EnemySpawner>();
    }

#if false
    private void Update()
    {
        // スポーンテスト
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartWave(0);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            StartBossWave();
        }
    }
#endif

    /// <summary>
    /// スポーン情報のスポーン比率をもとに生成する敵をランダム決定する
    /// </summary>
    /// <param name="spawnRatioData"></param>
    /// <returns></returns>
    private string GetRandomEnemyName(WavesDataScrObj.SpawnRatioData[] spawnRatioData)
    {
        // TODO:スポーン比率をもとにランダム決定
        return spawnRatioData[0].name;
    }

    /// <summary>
    /// スポナー稼働コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoWoking(WavesDataScrObj.WaveData data)
    {
        m_isWorking = true;
        Debug.Log("Enemy Spawner ON.");

        var generateCount = data.generateCount;
        while (0 < generateCount)
        {
            if (m_enemy == null)
            {
                // 新しい敵を生成
                var name = GetRandomEnemyName(data.spawnRatioData);
                m_enemy = m_enemySpawner.SpawnEnemy(name);
                generateCount--;
            }
            else if (m_enemy.isDead)
            {
                // 死亡したので破棄
                Destroy(m_enemy.gameObject);
                m_enemy = null;
            }
            yield return null;
        }

        m_isWorking = false;
        Debug.Log("Enemy Spawner OFF.");

    }

    /// <summary>
    /// ボス前の指定Waveを開始
    /// </summary>
    /// <param name="waveIndex">指定Wave</param>
    public void StartWave(int waveIndex)
    {
        if (m_isWorking)
            return;
        Debug.Assert((0 <= waveIndex) && (waveIndex < wavesCount));
        StartCoroutine(CoWoking(m_wavesData.waveData[waveIndex]));
    }

    /// <summary>
    /// ボスのWaveを開始
    /// </summary>
    public void StartBossWave()
    {
        if (m_isWorking)
            return;
        StartCoroutine(CoWoking(m_wavesData.bossWaveData));
    }
}
