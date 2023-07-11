using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SkeletonEnemy : Enemy
{
    [SerializeField, Networked] float health { get; set; }
    [SerializeField] bool isDead;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField]Transform target;
    [SerializeField] Animator anim;
    GameManager gm;
    bool takeDamage;
    [SerializeField] EnemyData data;
    [SerializeField] float attacsTimer;
    bool isOnHitRange;
    float damage;
    bool addKill;
    public override void Spawned()
    {
        gm = FindAnyObjectByType<GameManager>();
    }
    public override void FixedUpdateNetwork()
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
        if (attacsTimer > 0)
        {
            attacsTimer -= Time.deltaTime;
        }
        else
        {
            if (isOnHitRange && target != null && !isDead && Runner.IsSharedModeMasterClient)
                Attack();
        }
    }
    public override void Init(GameManager gm, Vector3 spawnPos)
    {
        this.gm = gm;

        transform.position = spawnPos;
        health = data.MaxHelath;
        damage = data.Damage;

        isDead = false;
    }
    public override void Attack()
    {
        attacsTimer = data.TimeBetweenAttacks;
        GameObject bullet = Runner.Spawn(bulletPrefab, transform.position, Quaternion.identity, Object.InputAuthority).gameObject;
        bullet.GetComponent<SkeletonBullet>().Init(target.position, damage);
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
        if (players.Count == 0)
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(_DetectPlayer());
        }
        else
        {
            int test;
            test = UnityEngine.Random.Range(0, players.Count);
            target = players[test];
        }
        yield return null;
    }
    public override void Move(Transform target)
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 5)
        {
            isOnHitRange = false;
            transform.position = Vector2.MoveTowards(transform.position, target.position, (data.MoveSpeed / 10) * Time.fixedDeltaTime);
        }
        else
        {
            isOnHitRange = true;
        }
    }

    private void Flip(Transform target)
    {
        if (target.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public override void TakeDamage(float damage, NetworkObject bullet)
    {
        RPC_TakeDamage(damage);
        if (bullet.StateAuthority == Runner.LocalPlayer)
        {
            if (isDead && !addKill)
            {
                addKill = true;
                gm.kills++;
            }
            else if (!isDead)
                gm.damage += damage;
        }
    }
    [Rpc]
    public void RPC_TakeDamage(float damage, RpcInfo info = default)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            anim.SetBool("Dead", true);
            Invoke(nameof(Despawn), 2);
        }
    }
    void Despawn()
    {
        Runner.Despawn(Object);
    }

}
