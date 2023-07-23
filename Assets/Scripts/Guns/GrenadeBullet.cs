using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Fusion;
class GrenadeBullet: NetworkBehaviour
{
    [SerializeField] ParticleSystem _particle;
    float _damage;
    Collider2D[] _hitEnemies;

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
        if (!collision.CompareTag("Enemy"))
            return;
        _hitEnemies = Physics2D.OverlapCircleAll(transform.position, 3);
        Instantiate(_particle, collision.transform.position, Quaternion.identity);
        if (_hitEnemies != null)
        {
            foreach (var hitEnemy in _hitEnemies)
            {
                if (hitEnemy.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.TakeDamage(_damage, Object);
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
