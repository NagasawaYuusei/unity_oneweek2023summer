using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class EnemyManager : KyawaLib.SingletonMonoBehaviour<EnemyManager>
{
    private GameObject BattleEnemy = null;
    [SerializeField] private Transform InstancePos;

    [SerializeField] private GameObject[] FirstEnemyPrefab;
    [SerializeField] private float FirstRatio = 0.6f;

    [SerializeField] private GameObject[] SecondEnemyPrefab;
    [SerializeField] private float SecondRatio = 0.4f;

    [SerializeField] private GameObject BossPrefab;
   
    private int Count = 0;

    public enum WaveState { FirstImpact, SecondImpact, ThirdImpact}
    //public WaveState wave = WaveState.FirstImpact;
    private WaveState m_State = WaveState.FirstImpact;

    private bool check = false;
    private int Max;

    private void Start()
    {
      
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            EnemyInstantiate(WaveState.FirstImpact, 3);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EnemyInstantiate(WaveState.SecondImpact, 4);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            EnemyInstantiate(WaveState.ThirdImpact, 1);
        }

        Debug.Log(check);

        if(check)
        {
            switch (m_State)
            {
                case WaveState.FirstImpact:
                    if (Count < Max)
                    {
                        SpawnWave(FirstEnemyPrefab, FirstRatio);
                    }
                    else if(BattleEnemy == null)
                    {
                        check = false;
                        Count = 0;
                    }
                break;

                case WaveState.SecondImpact:
                    if (Count < Max)
                    {
                        SpawnWave(SecondEnemyPrefab, SecondRatio);
                    }
                    else if(BattleEnemy == null)
                    {
                        check = false;
                        Count = 0;
                    }
                break;

                case WaveState.ThirdImpact:
                    if (Count < Max)
                    {
                        BossWave();
                    }
                    else if (BattleEnemy == null)
                    {
                        check = false;
                        Count = 0;
                    }
                    break;
            }
        }
    }

    
    /// <Summary>
    /// EnemyManagerが稼働しているかの判定関数.trueなら敵生成中もしくはまだ死亡していない.
    /// </Summary>
    public bool EnemyManagerState()
    {
        //返り値falseなら生成終了
        return check;

    }

    /// <Summary>
    /// 生成したいウェーブの敵と生成数を引数で入れてください。生成終了したかどうかはEnemyMangerState関数で管理。
    /// </Summary>
    public void EnemyInstantiate(WaveState State,int Num)
    {
        if (!check)
        {
            Max = Num;
            m_State = State;
            check = true;
        }      
    }

    private void SpawnWave(GameObject[] enemy,float ratio)
    {
        float RandomValue = Random.value;

        if (BattleEnemy == null)
        {
            if (RandomValue <= ratio)
            {
                BattleEnemy = Instantiate(enemy[0], new Vector3(InstancePos.position.x, InstancePos.position.y, InstancePos.position.z), Quaternion.identity);
            }
            else
            {
                BattleEnemy = Instantiate(enemy[1], new Vector3(InstancePos.position.x, InstancePos.position.y, InstancePos.position.z), Quaternion.identity);
            }
            Count++;
        }
    }

    private void BossWave()
    {
        BattleEnemy = Instantiate(BossPrefab, new Vector3(InstancePos.position.x, InstancePos.position.y, InstancePos.position.z), Quaternion.identity);
        Count++;
    }
}
