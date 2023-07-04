using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Bullet : NetworkBehaviour
{
    float damage;

    public override void FixedUpdateNetwork()
    {
        transform.Translate(Vector3.up * 20 * Time.fixedDeltaTime);
    }
    public void Init(float damage)
    {
        this.damage = damage;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Runner.Despawn(Object);
    }
}
