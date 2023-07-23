using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class EnemySpawner : NetworkBehaviour 
{
    [SerializeField] GameObject[] _enemyPrefabs;     //Prefabs of enemies
    [SerializeField] GameManager _gm;                //GameManager to Initiate the enemy
    [SerializeField] Transform[] _spawnPoints;       //Reference to all spawn points

    public void Spawn(int index)
    {
        GameObject enemy = Runner.Spawn(_enemyPrefabs[index], Vector2.zero, Quaternion.identity, Object.InputAuthority).gameObject;
        enemy.GetComponent<Enemy>().Init(_gm, _spawnPoints[Random.Range(0, _spawnPoints.Length)].position);
    }

}
