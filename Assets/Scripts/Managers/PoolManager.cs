using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/*
    Prefabs MUST be assigned in the following order:
    0) Player Bullet
    1) Grenade Bullet
    2) Grenade
    3) Medkit
    4) Ammo Boxes
 */
public class PoolManager : NetworkBehaviour
{
    [SerializeField] GameObject[] _resourcePrefabs;
    [SerializeField] GameObject[] _enemyPrefabs;
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] GameManager _gm;

    GameObject _select;

    public GameObject SpawnResource(int index)
    {
        Vector2 spawnPos = new Vector2(Random.Range(-25, 25), Random.Range(-25, 25));
        _select = Runner.Spawn(_resourcePrefabs[index], spawnPos, Quaternion.identity, Object.InputAuthority).gameObject;
        return _select;
    }
    public GameObject EnemySpawn(int index)
    {
        _select = Runner.Spawn(_enemyPrefabs[index], Vector2.zero, Quaternion.identity, Object.InputAuthority).gameObject;
        _select.GetComponent<Enemy>().Init(_gm, _spawnPoints[Random.Range(0, _spawnPoints.Length)].position);
        return _select;
    }
}

