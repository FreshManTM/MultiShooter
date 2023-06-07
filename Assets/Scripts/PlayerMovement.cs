using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDirection;
    public float moveSpeed = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(horizontal, vertical).normalized;
    }
    private void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}
