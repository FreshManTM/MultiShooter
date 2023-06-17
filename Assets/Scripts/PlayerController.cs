using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDirection;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Joystick joystick;
    SpriteRenderer spriter;
   
    [SerializeField] InputActionReference movement;

    [SerializeField] float health;
    [SerializeField] float maxHealth;
    Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    private void Update()
    {
        //moveDirection = new Vector2(joystick.Horizontal, joystick.Vertical);
        if (moveDirection.x != 0)
        {
            spriter.flipX = moveDirection.x < 0;
        }
    }
    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        Vector2 nextVec = moveDirection.normalized * moveSpeed* Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
    }
    void OnMove(InputValue value)
    {
        print("move direction " + moveDirection);
        moveDirection = value.Get<Vector2>();
        anim.SetFloat("Speed", moveDirection.magnitude);

    }
    public void TakeDamage(float damage)
    {
        if(health > 0)
        {
            health -= damage;
        }
        else
        {
            print("Player is dead");
        }
    }
}
