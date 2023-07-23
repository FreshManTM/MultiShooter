using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public float MaxHelath;
    public float Damage;
    public float MoveSpeed;
    public float TimeBetweenAttacks;
    public RuntimeAnimatorController AnimatorController;
}
