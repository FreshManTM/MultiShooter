using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class PumpedZombieFactory : EnemyFactory
{
    GameManager gm;
    Transform player;
    EnemyData data;
    PoolManager pool;

    GameObject zombie;
    public PumpedZombieFactory(PoolManager pool, GameManager gm, Transform player, EnemyData data) : base(pool, gm, player, data)
    {
        this.pool = pool;
        this.gm = gm;
        this.player = player;
        this.data = data;
    }

    public override void Spawn(Vector3 spawnPos)
    {
        zombie = pool.SpawnResource(0);
        
                // zombie.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        ZombieEnemy component = zombie.GetComponent<ZombieEnemy>();
        if (component == null)
            component = zombie.AddComponent<ZombieEnemy>();
        //component.Init(player, gm, data, spawnPos);
    }

}
