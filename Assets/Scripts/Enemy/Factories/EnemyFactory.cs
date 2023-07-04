using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public abstract class EnemyFactory: NetworkBehaviour
{
    public EnemyFactory(PoolManager pool, GameManager gm, Transform player, EnemyData data)
    {

    }
    public abstract void Spawn(Vector3 spawnPos);
}
