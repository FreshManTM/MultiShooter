using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SkeletonBullet : NetworkBehaviour
{
    Vector3 _target;     //The direction where the bullet should fly
    float _damage;       //Bullet damage
    float _lifeTimer;    //Bullet life time

    void Update()
    {
        if (_lifeTimer <= 0)
        {
            Runner.Despawn(GetComponent<NetworkObject>());
        }
        else
        {
            _lifeTimer -= Time.deltaTime;
        }
        transform.Translate(Vector3.up * 10 * Time.deltaTime);
    }
    public void Init(Vector3 target, float damage)
    {
        _target = target;
        _damage = damage;
        _lifeTimer = 4f;

        Vector3 toTarget = target - transform.position;
        float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            playerController.TakeDamage(_damage);
            Invoke(nameof(Despawn), 0.05f);
        }
    }
    void Despawn()
    {
        Runner.Despawn(Object);
    }
}
