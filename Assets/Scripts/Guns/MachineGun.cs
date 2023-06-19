using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MachineGun : Gun
{
    Vector3 offset = new Vector3(0, 0, -90);
    PoolManager pool;
    Transform muzzle;
    GunData gunData;
    float shotgunSpread = 8;
    public MachineGun(PoolManager pool, Transform muzzle, GunData gunData) : base(pool, muzzle, gunData)
    {
        this.pool = pool;
        this.muzzle = muzzle;
        this.gunData = gunData;
    }
    public override void Shoot()
    {
        GetBullet();
    }
}

