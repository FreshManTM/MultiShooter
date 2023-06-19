using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ShotGun : Gun
{
    Vector3 offset = new Vector3(0, 0, -90);
    PoolManager pool;
    Transform muzzle;
    GunData gunData;
    float shotgunSpread = 8;
    public  ShotGun(PoolManager pool, Transform muzzle, GunData gunData): base(pool, muzzle, gunData)
    {
        this.pool = pool;
        this.muzzle = muzzle;
        this.gunData = gunData;
    }
    public override void Shoot()
    {
       
        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = GetBullet();
            float bulletOffset = (i - (3 / 2)) * shotgunSpread + offset.z;
            bullet.transform.rotation = Quaternion.Euler(muzzle.transform.eulerAngles.x, muzzle.transform.eulerAngles.y, muzzle.transform.eulerAngles.z + bulletOffset);
        
            bullet.GetComponent<Bullet>().Init(gunData.Damage);
        }

    }
}

