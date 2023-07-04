using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SkeletonEnemy : Enemy
{
    [SerializeField] float health;
    [SerializeField] bool isDead;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField]Transform target;
    Animator anim;
    GameManager gm;
    
    [SerializeField] EnemyData data;

    [SerializeField] float attacsTimer;
    bool isOnHitRange;


    public override void Spawned()
    {

        print("Skeleton is spawned");
    }
    private void Update()
    {
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
        if (attacsTimer > 0)
        {
            attacsTimer -= Time.deltaTime;
        }
        else
        {
            if (isOnHitRange && target != null && Runner.IsSharedModeMasterClient)
                Attack();
        }
    }
    public override void Init(GameManager gm, Vector3 spawnPos)
    {
        this.gm = gm;

        transform.position = spawnPos;
        health = data.MaxHelath;
        anim = GetComponent<Animator>();

        anim.runtimeAnimatorController = data.AnimatorController;
        isDead = false;
    }
    public override void Attack()
    {
        attacsTimer = data.TimeBetweenAttacks;
        GameObject bullet = Runner.Spawn(bulletPrefab, Vector2.zero, Quaternion.identity, Object.InputAuthority).gameObject;
        bullet.transform.position = transform.position;
        bullet.GetComponent<SkeletonBullet>().Init(target.position, data.Damage);
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

    public override void TakeDamage(float damage)
    {
        health -= damage;
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

}
