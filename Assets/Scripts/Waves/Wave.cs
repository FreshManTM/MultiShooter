using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public abstract class Wave : NetworkBehaviour
{
    protected PoolManager _spawner;
    protected float _enemySpawnDelay;
    protected float _resourceSpawnDelay;
    public void StartSpawning(PoolManager spawner, WaveData data)
    {
        _spawner = spawner;
        _enemySpawnDelay = data.WaveTime / data.EnemiesPerWave;
        _resourceSpawnDelay = data.WaveTime / data.ResourcesPerWave;
        StartCoroutine(EnemySpawn());
        StartCoroutine(ResourceSpawn());
    }
    public abstract IEnumerator EnemySpawn();
    public abstract IEnumerator ResourceSpawn();

}
