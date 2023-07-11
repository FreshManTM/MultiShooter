using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Fusion;
class GrenadeBullet: NetworkBehaviour
{
    [SerializeField] ParticleSystem particle;
    float damage;
    Collider2D[] hittedEnemies;

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
        if (!collision.CompareTag("Enemy"))
            return;
        hittedEnemies = Physics2D.OverlapCircleAll(transform.position, 3);
        Instantiate(particle, collision.transform.position, Quaternion.identity);
        if (hittedEnemies != null)
        {
            foreach (var enemy in hittedEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponent<Enemy>().TakeDamage(damage, Object);
                }
            }
            Runner.Despawn(Object);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;
        Runner.Despawn(Object);
    }
}
