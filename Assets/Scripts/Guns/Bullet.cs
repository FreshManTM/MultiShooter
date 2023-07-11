using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Bullet : NetworkBehaviour
{
    float damage;
    GameManager gm;

    public override void FixedUpdateNetwork()
    {
        transform.Translate(Vector3.up * 20 * Time.fixedDeltaTime);
    }
    public void Init(float damage)
    {
        this.damage = damage;
        gm = FindObjectOfType<GameManager>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && Object.StateAuthority == Runner.LocalPlayer)
        {
            collision.GetComponent<Enemy>().TakeDamage(damage, Object);
            gameObject.SetActive(false);
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
