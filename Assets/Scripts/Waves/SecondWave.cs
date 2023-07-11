using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondWave : Wave
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
        float randomValue = Random.value;
        int enemyNumber;
        if (randomValue < .6f)
            enemyNumber = 0;
        else
            enemyNumber = 2;

        spawner.EnemySpawn(enemyNumber);
        yield return new WaitForSeconds(enemySpawnDelay);
        StartCoroutine(EnemySpawn());
    }

    public override IEnumerator ResourceSpawn()
    {
        float randomValue = Random.value;
        int resourceNumber;
        if (randomValue < .6f)
            resourceNumber = 4;
        else
            resourceNumber = 3;
        spawner.SpawnResource(resourceNumber);
        yield return new WaitForSeconds(resourceSpawnDelay);
        StartCoroutine(ResourceSpawn());

    }
}
