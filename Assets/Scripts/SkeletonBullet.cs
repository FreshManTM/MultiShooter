using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBullet : MonoBehaviour
{
    Vector3 target;
    float damage;
    float lifeTimer;
    void Update()
    {
        if (lifeTimer <= 0)
            gameObject.SetActive(false);
        else
        {
            lifeTimer -= Time.deltaTime;
        }
        transform.Translate(Vector3.up * 10 * Time.deltaTime);
    }
    public void Init(Vector3 target, float damage)
    {
        this.target = target;
        this.damage = damage;
        lifeTimer = 4f;

        Vector3 toTarget = target - transform.position;
        float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
