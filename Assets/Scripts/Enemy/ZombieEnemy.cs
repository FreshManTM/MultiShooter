using System;
using UnityEngine;
using Fusion;
using System.Collections;
using System.Collections.Generic;

class ZombieEnemy : Enemy
{
    [SerializeField] float health;
    bool isDead;
    Transform target;
    Animator anim;
    GameManager gm;
    [SerializeField] EnemyData data;

    [SerializeField] float attacksTimer;
    bool isOnHitRange;

    private void Update()
    {
        if (attacksTimer > 0)
        {
            attacksTimer -= Time.deltaTime;
        }
        else
        {
            if (isOnHitRange && target != null)
                Attack();
        }
    }
    private void FixedUpdate()
    {
        if (target == null)
        {
            StartCoroutine(_DetectPlayer());
        }
        else if (!isDead)
        {
            Flip(target);
            Move(target);

        }
    }

    public override void Init(GameManager gm, Vector3 spawnPos)
    {
        //this.target = target;
        this.gm = gm;

        transform.position = spawnPos;
        health = data.MaxHelath;
        anim = GetComponent<Animator>();

        isDead = false;
    }

    IEnumerator _DetectPlayer()
    {  
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25f);
        List<Transform> players = new List<Transform>();
        foreach (var item in colliders)
        {
            if (item.CompareTag("Player"))
            {
                print("Player detected");
                players.Add(item.transform);
            }
        }
        if(players.Count == 0)
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(_DetectPlayer());
        }
        else
        {
            int test;
            test = UnityEngine.Random.Range(0, players.Count);
            target = players[test];
            StopCoroutine(_DetectPlayer());
        }
        yield return null;
    }
    public override void Attack()
    {
        print("player hitted");
        target.gameObject.GetComponentInChildren<PlayerController>().TakeDamage(data.Damage);
        attacksTimer = data.TimeBetweenAttacks;
    }

    public override void Move(Transform target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, (data.MoveSpeed / 10) * Time.fixedDeltaTime);
    }

    private void Flip(Transform target)
    {
        if (target.transform.position.x < transform.position.x)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else
            transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public override void TakeDamage(float damage)
    {
        RPC_TakeDamage(damage);
    }
    [Rpc]
    public void RPC_TakeDamage(float damage, RpcInfo info = default)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            anim.SetBool("Dead", true);
            gm.kills++;
            print("This is death");
            Invoke(nameof(Despawn), 2);
        }
    }
    void Despawn()
    {
        print("This is despawn");
        Runner.Despawn(Object);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isOnHitRange = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isOnHitRange = false;
    }
}

