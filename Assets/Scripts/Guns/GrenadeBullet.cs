using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GrenadeBullet:MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    float damage;
    Collider2D[] hittedEnemies;

    private void FixedUpdate()
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
                    enemy.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
            Destroy(gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;
        Destroy(gameObject);
    }
}
