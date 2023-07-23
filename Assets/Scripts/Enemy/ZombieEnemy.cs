using System;
using UnityEngine;
using Fusion;
using System.Collections;
using System.Collections.Generic;

class ZombieEnemy : Enemy
{
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
            if (_isOnHitRange && _target != null)
                Attack();
        }
    }

    public override void Init(GameManager gm, Vector3 spawnPos)
    {
        _gm = gm;
        transform.position = spawnPos;
    }

    public override void Attack()
    {
        _target.gameObject.GetComponent<PlayerController>().TakeDamage(_data.Damage);
        _attacksTimer = _data.TimeBetweenAttacks;
    }

    public override void Move(Transform target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, (_data.MoveSpeed / 10) * Time.fixedDeltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == _target.gameObject)
        {
            _isOnHitRange = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == _target.gameObject)
        { 
            _isOnHitRange = false; 
        }
    }
}

