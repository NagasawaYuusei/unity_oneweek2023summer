using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System;

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
    /// 現状は生成する度に関数呼んでますが、配列にしてスポナー稼働開始時に一度に生成した方がいいです
    /// </summary>
    /// <param name="spawnRatioData"></param>
    /// <returns></returns>
    private string GetRandomEnemyName(WavesDataScrObj.SpawnRatioData[] spawnRatioData)
    {
        Debug.Assert(spawnRatioData != null);
        Debug.Assert(0 < spawnRatioData.Length);

        // 1体しかいない
        if (spawnRatioData.Length == 1)
            return spawnRatioData[0].name;

        // 敵と乱数の結びつけ
        var ratioSum = 0; // 比率の合計
        var tuples = new List<Tuple<string, int>>();
        foreach (var data in spawnRatioData)
        {
            // 比率0の敵は生成しない
            if (0 < data.ratio)
            {
                ratioSum += data.ratio;
                tuples.Add(new(data.name, ratioSum));
            }
        }

        // 乱数生成
        var rand = UnityEngine.Random.Range(0, ratioSum);
        rand++; // 1〜ratioSum+1の数に

        // 乱数をもとに敵を決定
        var name = "!--NONE--!";
        foreach (var tuple in tuples)
        {
            if (rand <= tuple.Item2)
            {
                name = tuple.Item1;
                break;
            }
        };
        return name;
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
        while (m_isWorking)
        {
            if (m_enemy == null)
            {
                // 新しい敵を生成
                var name = GetRandomEnemyName(data.spawnRatioData);
                m_enemy = m_enemySpawner.SpawnEnemy(name);
                if (m_enemy == null)
                    break; // 強制終了
                generateCount--;
            }
            else if (m_enemy.isDead)
            {
                // 死亡したので破棄
                Destroy(m_enemy.gameObject);
                m_enemy = null;
                // 死亡したのが最後の敵であれば終了
                if (generateCount <= 0)
                    break;
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

    /// <summary>
    /// Wave中断（ゲームオーバー）
    /// </summary>
    public void StopWave()
    {
        m_isWorking = false;
    }
}
