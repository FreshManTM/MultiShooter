using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Gun", menuName ="Gun")]
public class GunData : ScriptableObject
{
    public enum GunType
    {
        SawedOff,
        MachineGun,
        ShotGun
    }
    [Header("Info")]
    public GunType Type;
    public float Damage;
    public float Distance;
    [Header("Shooting")]
    public int MagazineSize;
    public float TimeBetweenShots;
    public float ReloadTime;
    [Header("Assign Area")]
    public GameObject BulletPrefab;
    public Sprite GunSprite;
}
