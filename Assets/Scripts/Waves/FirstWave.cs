using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstWave : Wave
{
    PoolManager spawner;
    float enemySpawnDelay;
    float resourceSpawnDelay;

    public override void Spawn(PoolManager spawner, WaveData data)
    {
        this.spawner = spawner;
        enemySpawnDelay = data.waveTime / data.enemiesPerWave;
        resourceSpawnDelay = data.waveTime / data.resourcesPerWave;
        
        StartCoroutine(EnemySpawn());
        StartCoroutine(ResourceSpawn());

    }
    public override IEnumerator EnemySpawn()
    {
        print("we spawn enemy. Fist wave");
        spawner.EnemySpawn(0);
        yield return new WaitForSeconds(enemySpawnDelay);
        StartCoroutine(EnemySpawn());
    }

    public override IEnumerator ResourceSpawn()
    {
        spawner.SpawnResource(4);
        yield return new WaitForSeconds(resourceSpawnDelay);
        StartCoroutine(ResourceSpawn());

    }
}
