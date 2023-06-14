using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonEnemy : Enemy
{
    [SerializeField] float health;
    bool isDead;
    Transform target;
    Animator anim;
    GameManager gm;
    EnemyData data;
    PoolManager pool;
    [SerializeField] float attacsTimer;
    bool isOnHitRange;

    GameObject player;
    private void Update()
    {
        if (attacsTimer > 0)
        {
            attacsTimer -= Time.deltaTime;
        }
        else
        {
            if (isOnHitRange)
                Attack();
        }
    }
    private void FixedUpdate()
    {
        if (!isDead)
            Move(target);
    }
    public void Init(Transform target, GameManager gm, EnemyData data, Vector3 spawnPos, PoolManager pool)
    {
        this.target = target;
        this.gm = gm;
        this.data = data;
        this.pool = pool;

        transform.position = spawnPos;
        health = data.MaxHelath;
        anim = GetComponent<Animator>();

        anim.runtimeAnimatorController = data.AnimatorController;
    }
    public override void Attack()
    {
        attacsTimer = data.TimeBetweenAttacks;
        GameObject bullet = pool.GetBullet(1);
        bullet.transform.position = transform.position;
        bullet.GetComponent<SkeletonBullet>().Init(target.position, data.Damage);
    }

    public override void Move(Transform target)
    {
        if (target.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);

        }
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

    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            anim.SetBool("Dead", true);
            gm.AddKill();
            Invoke("Dead", 2);
        }
    }
    void Dead()
    {
        gameObject.SetActive(false);
    }

}
