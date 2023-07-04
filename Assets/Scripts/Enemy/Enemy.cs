using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public abstract class Enemy : NetworkBehaviour 
{
    public abstract void Move(Transform target);
    public abstract void Attack();
    public abstract void TakeDamage(float damage);
    public abstract void Init(GameManager gm, Vector3 spawnPos);
}
