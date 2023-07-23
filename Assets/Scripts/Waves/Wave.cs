using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public abstract class Wave : NetworkBehaviour
{
    public abstract void StartSpawning(PoolManager spawner, WaveData data);
    public abstract IEnumerator EnemySpawn();
    public abstract IEnumerator ResourceSpawn();

}
