using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    Vector3 offset = new Vector3(0, 0, -90);
    PoolManager pool;
    Transform muzzle; 
    GunData gunData;
    public Gun()
    {

    }
    public Gun(PoolManager pool, Transform muzzle, GunData gunData)
    {
        this.pool = pool;
        this.muzzle = muzzle;
        this.gunData = gunData;
    }
    public abstract void Shoot();
    protected GameObject GetBullet()
    {
        GameObject bullet = pool.Get(1);
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = Quaternion.Euler(muzzle.transform.rotation.eulerAngles + offset);

        bullet.GetComponent<Bullet>().Init(gunData.Damage);
        return bullet;
    }
}
