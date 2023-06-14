using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void Move(Transform target);
    public abstract void Attack();
    public abstract void TakeDamage(float damage);
}
