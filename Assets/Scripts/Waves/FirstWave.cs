using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstWave : Wave
{
    public override IEnumerator EnemySpawn()
    {
        _spawner.EnemySpawn(0);
        yield return new WaitForSeconds(_enemySpawnDelay);
        StartCoroutine(EnemySpawn());
    }

    public override IEnumerator ResourceSpawn()
    {
        _spawner.SpawnResource(4);
        yield return new WaitForSeconds(_resourceSpawnDelay);
        StartCoroutine(ResourceSpawn());
    }
}
