using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
public class PlayerController : NetworkBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDirection;
    [SerializeField] float moveSpeed = 10f;
    SpriteRenderer spriter;
   
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    GameManager gm;
    Animator anim;
    bool isDead;
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gm = FindObjectOfType<GameManager>();
        health = maxHealth;

    }
    private void Update()
    {
        if (moveDirection.x != 0)
        {
            spriter.flipX = moveDirection.x < 0;
        }
    }
    void ChangeSpire()
    {
        spriter.sprite = null;
    }
    public override void FixedUpdateNetwork()
    {
        if(!isDead && rb != null)
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
    void OnMove(InputValue value)
    {
        if (!Object.HasInputAuthority)
            return;
        moveDirection = value.Get<Vector2>();
        anim.SetFloat("Speed", moveDirection.magnitude);

    }
    public void TakeDamage(float damage)
    {
        if(health - damage > 0)
        {
            health -= damage;
            if(Object.HasInputAuthority)
                gm.health = health;

        }
        else if(!isDead)
        {
            health = 0;
            isDead = true;
            anim.SetTrigger("Dead");
            Invoke(nameof(Despawn), .5f);
            if (Object.HasInputAuthority)
            {
                gm.health = health;
                gm.Death();
                //gm.Death();
            }
        }
    }
    void Despawn()
    {
        Runner.Despawn(Object);
    }
    public void AddHealth(float heal)
    {
        health += heal;
        if (health > maxHealth)
            health = maxHealth;
        gm.health = health;

    }
}
