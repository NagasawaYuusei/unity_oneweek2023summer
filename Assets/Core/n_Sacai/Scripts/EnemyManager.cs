using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class EnemyManager : KyawaLib.SingletonMonoBehaviour<EnemyManager>
{
    private GameObject BattleEnemy = null;                      //生成したエネミーを入れる
    [SerializeField] private Transform InstancePos;             //生成ポジション

    [SerializeField] private GameObject[] FirstEnemyPrefab;     //1ウェーブ目の敵配列
    [SerializeField] private float FirstRatio = 0.6f;           //生成比率

    [SerializeField] private GameObject[] SecondEnemyPrefab;    //2ウェーブ目の敵配列
    [SerializeField] private float SecondRatio = 0.4f;          

    [SerializeField] private GameObject BossPrefab;             //ボス専用
   
    private int Count = 0;                                      //現在何体生成したかをカウント

    public enum WaveState { FirstImpact, SecondImpact, ThirdImpact}     //ウェーブステート（これを引数で指定して）
 
    private WaveState m_State = WaveState.FirstImpact;

    private bool check = false;                                 //敵生成中はtrue、生成し終わるとfalse
    private int Max;                                                    

    private void Start()
    {
      
    }
    
    private void Update()
    {
        //デバッグ用操作
        //Fキーでサコ敵、Eキーで強敵、Gキーでボス
        if (Input.GetKeyDown(KeyCode.F))
        {
            EnemyInstantiate(WaveState.FirstImpact, 5);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EnemyInstantiate(WaveState.SecondImpact, 4);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            EnemyInstantiate(WaveState.ThirdImpact, 1);
        }

        if(check)       //生成フラグが立ってれば生成開始
        {
            switch (m_State)
            {
                //指定されたウェーブに基づき条件分岐
                case WaveState.FirstImpact:
                    if (Count < Max)
                    {
                        SpawnWave(FirstEnemyPrefab, FirstRatio);
                    }
                    else if(BattleEnemy == null)    //全ての敵を生成し終わり、画面内の敵も死亡したら生成フラグをfalseに
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


    /// <Summary>
    /// 生成したいウェーブの敵配列と比率を与えたら敵を生成
    /// </Summary>
    private void SpawnWave(GameObject[] enemy,float ratio)
    {
        float RandomValue = Random.value;

        if (BattleEnemy == null)        //今は敵が死亡したら新しく次の敵を生成。このif文を変えたら自由なタイミングで生成可能。
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

    /// <Summary>
    /// ボス専用生成関数。引数なし
    /// </Summary>
    private void BossWave()
    {
        BattleEnemy = Instantiate(BossPrefab, new Vector3(InstancePos.position.x, InstancePos.position.y, InstancePos.position.z), Quaternion.identity);
        Count++;
    }
}
