using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameManager gm;
    [SerializeField] Transform player;
    [SerializeField] EnemyData[] data;
    [SerializeField] PoolManager pool;

    [SerializeField] float spawnTime;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField]float spawnTimer;

    EnemyFactory zombieFactory;
    private void Update()
    {

        if(spawnTimer <= 0)
        {
            spawnTimer = spawnTime;
            //zombieFactory = new NormalZombieFactory(pool, gm, player, data[0]);
            zombieFactory = new PumpedZombieFactory(pool, gm, player, data[1]);
            //zombieFactory = new SkeletonFactory(pool, gm, player, data[2]);

            zombieFactory.Spawn(spawnPoints[Random.Range(0, spawnPoints.Length)].position);
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }
}
