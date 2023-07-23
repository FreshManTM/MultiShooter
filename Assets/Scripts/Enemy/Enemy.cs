using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public abstract class Enemy : NetworkBehaviour 
{
    protected float _health;
    protected GameManager _gm;
    protected bool _addKill;        //Controls adding the kill only ones
    protected Animator _anim;

    protected Transform _target;
    protected bool _isDead;
    protected float _attacksTimer;    //Timer that counts time between attacks
    protected bool _isOnHitRange;                      //Checks if zombie is near enough to the player

    public override void Spawned()
    {
        _gm = FindObjectOfType<GameManager>();
        _anim = GetComponent<Animator>();
    }
    public override void FixedUpdateNetwork()
    {
        if (_target == null)
        {
            _target = DetectPlayer();
        }
        if (!_isDead)
        {
            Flip();
            Move(_target);
        }
    }
    public abstract void Init(GameManager gm, Vector3 spawnPos);
    public abstract void Move(Transform target);
    public abstract void Attack();
    protected Transform DetectPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25f);
        List<Transform> players = new List<Transform>();
        foreach (var item in colliders)
        {
            if (item.CompareTag("Player"))
            {
                players.Add(item.transform);
            }
        }

        if (players.Count > 0)
        {
            int playerNumber = UnityEngine.Random.Range(0, players.Count);
            return players[playerNumber];
        }
        return null;
    }
    protected void Flip()
    {
        if (_target.transform.position.x < transform.position.x)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
    }
    public void TakeDamage(float damage, NetworkObject bullet)
    {
        RPC_TakeDamage(damage);
        if (bullet.StateAuthority == Runner.LocalPlayer)
        {
            if (_isDead && !_addKill)
            {
                _addKill = true;
                _gm.Damage += damage;
                _gm.Kills++;
            }
            else if (!_isDead)
            {
                _gm.Damage += damage;
            }
        }
    }
    [Rpc]
    public void RPC_TakeDamage(float damage, RpcInfo info = default)
    {
        _health -= damage;
        if (_health <= 0 && !_isDead)
        {
            _isDead = true;
            _anim = GetComponent<Animator>();
            _anim.SetBool("Dead", true);
            Invoke(nameof(Despawn), 2);
        }
    }
    void Despawn()
    {
        Runner.Despawn(Object);
    }
}
