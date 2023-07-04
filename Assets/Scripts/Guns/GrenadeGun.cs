using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeGun : Gun
{
    Vector3 offset = new Vector3(0, 0, -90);
    PoolManager pool;
    Transform muzzle;
    GunData gunData;
    public GrenadeGun(PoolManager pool, Transform muzzle, GunData gunData) : base(pool, muzzle, gunData)
    {
        this.pool = pool;
        this.muzzle = muzzle;
        this.gunData = gunData;
    }
    public override void Shoot()
    {
        GameObject bullet = pool.SpawnResource(2);
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = Quaternion.Euler(muzzle.transform.rotation.eulerAngles + offset);
        bullet.GetComponent<GrenadeBullet>().Init(gunData.Damage);
    }
}
