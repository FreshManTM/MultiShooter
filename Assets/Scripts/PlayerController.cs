using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDirection;
    Vector2 moveInput;
    Vector2 moveInputJ;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Joystick joystick;
    SpriteRenderer spriter;
    PlayerInput playerInput;
   
    [SerializeField] InputActionReference movement;

    [SerializeField] float health;
    [SerializeField] float maxHealth;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        spriter = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    private void Update()
    {
        moveDirection = new Vector2(joystick.Horizontal, joystick.Vertical);
        //moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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
    //Vector2 keepV;
    //void OnMove(InputValue value)
    //{
    //    print("MOVING!!!");
    //    keepV = value.Get<Vector2>();
    //    moveDirection = value.Get<Vector2>();

    //}

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
