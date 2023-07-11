using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/*
    Prefabs MUST be assigned in the following order:
    0) Player Bullet
    1) Skeleton Bullet
    2) Grenade
    3) Medkit
    4) Ammo Boxes
 */
public class PoolManager : NetworkBehaviour
{
    [SerializeField] GameObject[] resourcePrefabs;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameManager gm;

    GameObject select;

    public GameObject SpawnResource(int index)
    {
        Vector2 spawnPos = new Vector2(Random.Range(-25, 25), Random.Range(-25, 25));
        select = Runner.Spawn(resourcePrefabs[index], spawnPos, Quaternion.identity, Object.InputAuthority).gameObject;
        print("Resource spawned");
        return select;
    }
    public GameObject EnemySpawn(int index)
    {
        select = Runner.Spawn(enemyPrefabs[index], Vector2.zero, Quaternion.identity, Object.InputAuthority).gameObject;
        select.GetComponent<Enemy>().Init(gm, spawnPoints[Random.Range(0, spawnPoints.Length)].position);
        return select;
    }
}

