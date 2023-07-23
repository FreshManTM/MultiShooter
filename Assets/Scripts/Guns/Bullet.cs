using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Bullet : NetworkBehaviour
{
    float _damage;

    public override void FixedUpdateNetwork()
    {
        transform.Translate(Vector3.up * 20 * Time.fixedDeltaTime);
    }
    public void Init(float damage)
    {
        _damage = damage;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy) && Object.StateAuthority == Runner.LocalPlayer)
        {
            enemy.TakeDamage(_damage, Object);
            Invoke(nameof(Despawn), 0.05f);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Area"))
        {
            Invoke(nameof(Despawn), 0.05f);
        }
    }

    void Despawn()
    {
        Runner.Despawn(Object);
    }
}
