using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameObject BattleEnemy = null;
    [SerializeField] private Transform InstancePos;
    [SerializeField] private Transform[] WaitPos = new Transform[2];

    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private int FirstWaveEnemyNum;

    //private List<GameObject> Enemies = new List<GameObject>();

    private int Count;
    private bool canInstance = true;
 

    void Update()
    {
        FirstWave();
    }

    public void FirstWave()
    {
       if(FirstWaveEnemyNum > Count)
       {
            if(BattleEnemy == null)
            {
                BattleEnemy = Instantiate(EnemyPrefab, new Vector3(InstancePos.position.x, InstancePos.position.y, InstancePos.position.z), Quaternion.identity);
                Count++;
            }
       }

      
    }

}
