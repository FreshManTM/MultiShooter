using System;
using UnityEngine;


class ZombieEnemy : Enemy
{
    [SerializeField] float health;
    bool isDead;
    Transform target;
    Animator anim;
    GameManager gm;
    EnemyData zombieData;

    [SerializeField]float attacsTimer;
    bool isOnHitRange;

    private void Update()
    {
        if (attacsTimer > 0)
        {
            attacsTimer -= Time.deltaTime;
        }
        else
        {
            if(isOnHitRange)
                Attack();
        }
    }
    private void FixedUpdate()
    {
        if (!isDead)
            Move(target);
    }

    public void Init(Transform target, GameManager gm, EnemyData data, Vector3 spawnPos)
    {
        this.target = target;
        this.gm = gm;
        this.zombieData = data;

        transform.position = spawnPos;
        health = data.MaxHelath;
        anim = GetComponent<Animator>();

        anim.runtimeAnimatorController = data.AnimatorController;
        isDead = false;
    }
    public override void Attack()
    {
        print("player hitted");
        target.gameObject.GetComponent<PlayerController>().TakeDamage(zombieData.Damage);
        attacsTimer = zombieData.TimeBetweenAttacks;
    }

    public override void Move(Transform target)
    {
        if(target.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);

        }
        transform.position = Vector2.MoveTowards(transform.position, target.position, (zombieData.MoveSpeed / 10)* Time.fixedDeltaTime);
    }
    public override void TakeDamage(float damage)
    {     
        health -= damage;
        print("Take damage " + damage);
        if (health <= 0 && !isDead)
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOnHitRange = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOnHitRange = false;
        }
    }
}

