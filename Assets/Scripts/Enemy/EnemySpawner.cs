using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class EnemySpawner : NetworkBehaviour 
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameManager gm;
    [SerializeField] Transform player;
    [SerializeField] Transform[] spawnPoints;

    //public override void FixedUpdateNetwork()
    //{
    //    if (Runner.IsSharedModeMasterClient)
    //    {
    //        if (spawnTimer <= 0)
    //        {
    //            spawnTimer = spawnTime;
    //            print("IsServer spawning enemy");
    //            Spawn(0);

    //        }
    //        else
    //        {
    //            spawnTimer -= Time.fixedDeltaTime;
    //        }
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }

    //}
    public void Spawn(int index)
    {
        GameObject enemy = Runner.Spawn(enemyPrefabs[index], Vector2.zero, Quaternion.identity, Object.InputAuthority).gameObject;
        enemy.GetComponent<Enemy>().Init(gm, spawnPoints[Random.Range(0, spawnPoints.Length)].position);
    }

}
