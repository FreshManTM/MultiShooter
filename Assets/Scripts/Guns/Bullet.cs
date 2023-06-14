using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float damage;
    bool isReady;
    private void FixedUpdate()
    {
        // if(isReady)
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
        collision.GetComponent<Enemy>().TakeDamage(damage);
        gameObject.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        gameObject.SetActive(false);
    }
}
