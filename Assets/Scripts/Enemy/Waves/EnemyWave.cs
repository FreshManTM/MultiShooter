using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public abstract class EnemyWave : NetworkBehaviour
{
    public abstract void Spawn(PoolManager spawner, WaveData data);
    public abstract IEnumerator EnemySpawn();
    public abstract IEnumerator ResourceSpawn();

}
