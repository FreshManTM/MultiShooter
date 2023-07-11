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
    [SerializeField] Joystick moveJoystick;
    [SerializeField]GameManager gm;
    Animator anim;
    bool isDead;
    public override void Spawned()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        StartCoroutine(FindGameManager());

        health = maxHealth;
        moveJoystick = GameObject.Find("Movement Joystick").GetComponent<Joystick>();
    }
    IEnumerator FindGameManager()
    {
        gm = FindObjectOfType<GameManager>();
        if(gm == null)
        {
            yield return new WaitForSeconds(.1f);
            StartCoroutine(FindGameManager());
        }
        else
        {
            yield return null;
        }
    }
    void ChangeSpire()
    {
        spriter.sprite = null;
    }
    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority)
            return;
        Move();
        anim.SetFloat("Speed", moveDirection.magnitude);

        if (!isDead && rb != null)
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

        if (moveDirection.x != 0)
        {
            spriter.flipX = moveDirection.x < 0;
        }

    }

    private void Move()
    {
        if (Mathf.Abs(moveJoystick.Horizontal) < .01f || Mathf.Abs(moveJoystick.Vertical) < .01f)
        {
            moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else
        {
            moveDirection = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical);
        }
    }

    //void OnMove(InputValue value)
    //{
    //    if (!Object.HasInputAuthority)
    //        return;
    //    moveDirection = value.Get<Vector2>();
    //    anim.SetFloat("Speed", moveDirection.magnitude);

    //}
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
