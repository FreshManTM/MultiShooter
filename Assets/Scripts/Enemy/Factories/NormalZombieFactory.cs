using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombieFactory : EnemyFactory
{
    GameManager gm;
    Transform player;
    EnemyData data;
    PoolManager pool;

    GameObject zombie;
    public NormalZombieFactory(PoolManager pool, GameManager gm, Transform player, EnemyData data) : base(pool, gm, player, data)
    {
        this.pool = pool;
        this.gm = gm;
        this.player = player;
        this.data = data;
    }
    public override void Spawn(Vector3 spawnPos)
    {
        zombie = pool.Get(0);
        ZombieEnemy component = zombie.GetComponent<ZombieEnemy>();
        if (component == null)
            component = zombie.AddComponent<ZombieEnemy>();
        component.Init(player, gm, data, spawnPos);
    }
}
