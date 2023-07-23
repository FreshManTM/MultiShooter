using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocalPlayerMovement : MonoBehaviour
{
    Rigidbody2D _rb;
    Vector2 _moveDirection;
    [SerializeField] float _moveSpeed = 10f;
    SpriteRenderer _spriter;

    [SerializeField] float _health;
    [SerializeField] float _maxHealth;
    [SerializeField] Joystick _moveJoystick;
    [SerializeField] GameManager _gm;
    Animator _anim;
    bool _isDead;

    bool _flipPlayer;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _moveJoystick = GameObject.Find("Movement Joystick").GetComponent<Joystick>();
        _anim = GetComponent<Animator>();
        _spriter = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Move();
        _anim.SetFloat("Speed", _moveDirection.magnitude);
        if (!_isDead && _rb != null)
            _rb.velocity = new Vector2(_moveDirection.x * _moveSpeed, _moveDirection.y * _moveSpeed);

        if (_moveDirection.x < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if(_moveDirection.x > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void Move()
    {
        if (Mathf.Abs(_moveJoystick.Horizontal) > .01f || Mathf.Abs(_moveJoystick.Vertical) > .01f)
        {
            _moveDirection = new Vector2(_moveJoystick.Horizontal, _moveJoystick.Vertical);
        }
        else
        {
            _moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }
}
