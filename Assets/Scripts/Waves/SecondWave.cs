using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondWave : Wave
{
    public override IEnumerator EnemySpawn()
    {
        float randomValue = Random.value;
        int enemyNumber;
        if (randomValue < .6f)
            enemyNumber = 0;
        else
            enemyNumber = 2;

        _spawner.EnemySpawn(enemyNumber);
        yield return new WaitForSeconds(_enemySpawnDelay);
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
        _spawner.SpawnResource(resourceNumber);
        yield return new WaitForSeconds(_resourceSpawnDelay);
        StartCoroutine(ResourceSpawn());

    }
}
