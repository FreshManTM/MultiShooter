using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFactory : EnemyFactory
{
    GameManager gm;
    Transform player;
    EnemyData data;
    PoolManager pool;

    GameObject zombie;
    public SkeletonFactory(PoolManager pool, GameManager gm, Transform player, EnemyData data) : base(pool, gm, player, data)
    {
        this.pool = pool;
        this.gm = gm;
        this.player = player;
        this.data = data;
    }
    public override void Spawn(Vector3 spawnPos)
    {
        zombie = pool.GetEnemy();
        SkeletonEnemy component = zombie.AddComponent<SkeletonEnemy>();
        component.Init(player, gm, data, spawnPos, pool);
    }
}
