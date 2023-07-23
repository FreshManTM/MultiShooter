using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;
public class PlayerController : NetworkBehaviour
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
    public override void Spawned()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriter = GetComponentInChildren<SpriteRenderer>();
        _anim = GetComponentInChildren<Animator>();
        StartCoroutine(_FindGameManager());

        _health = _maxHealth;
        _moveJoystick = GameObject.Find("Movement Joystick").GetComponent<Joystick>();
    }
    IEnumerator _FindGameManager()
    {
        _gm = FindObjectOfType<GameManager>();
        if(_gm == null)
        {
            yield return new WaitForSeconds(.1f);
            StartCoroutine(_FindGameManager());
        }
        else
        {
            yield return null;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority)
            return;
        
        Move();
        Flip();

        _anim.SetFloat("Speed", _moveDirection.magnitude);
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

        if (!_isDead && _rb != null)
            _rb.velocity = new Vector2(_moveDirection.x * _moveSpeed, _moveDirection.y * _moveSpeed);
    }
    private void Flip()
    {
        if (_moveDirection.x < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (_moveDirection.x > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void TakeDamage(float damage)
    {
        if(_health - damage > 0)
        {
            _health -= damage;
            if(Object.HasInputAuthority)
                _gm.Health = _health;

        }
        else if(!_isDead)
        {
            _health = 0;
            _isDead = true;
            _anim.SetTrigger("Dead");
            Invoke(nameof(Despawn), .5f);
            if (Object.HasInputAuthority)
            {
                _gm.Health = _health;
                _gm.Death();
            }
        }
    }
    void Despawn()
    {
        Runner.Despawn(Object);
    }
    public void AddHealth(float heal)
    {
        _health += heal;
        if (_health > _maxHealth)
            _health = _maxHealth;
        _gm.Health = _health;
    }
}
