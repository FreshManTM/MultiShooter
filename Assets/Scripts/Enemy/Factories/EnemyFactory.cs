using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyFactory
{
    public EnemyFactory(PoolManager pool, GameManager gm, Transform player, EnemyData data)
    {

    }
    public abstract void Spawn(Vector3 spawnPos);
}
