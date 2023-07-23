using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SkeletonEnemy : Enemy
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] EnemyData _data;

    public override void Spawned()
    {
        base.Spawned();
        _health = _data.MaxHelath;
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (_attacksTimer > 0)
        {
            _attacksTimer -= Time.deltaTime;
        }
        else
        {
            if (_isOnHitRange && _target != null && !_isDead && Runner.IsSharedModeMasterClient)
                Attack();
        }
    }
    public override void Init(GameManager gm, Vector3 spawnPos)
    {
        _gm = gm;

        transform.position = spawnPos;
        _health = _data.MaxHelath;
    }
    public override void Attack()
    {
        _attacksTimer = _data.TimeBetweenAttacks;
        GameObject bullet = Runner.Spawn(_bulletPrefab, transform.position, Quaternion.identity, Object.InputAuthority).gameObject;
        bullet.GetComponent<SkeletonBullet>().Init(_target.position, _data.Damage);
    }

    public override void Move(Transform target)
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 5)
        {
            _isOnHitRange = false;
            transform.position = Vector2.MoveTowards(transform.position, target.position, (_data.MoveSpeed / 10) * Time.fixedDeltaTime);
        }
        else
        {
            _isOnHitRange = true;
        }
    }
}
