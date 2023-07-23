using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MachineGun : Gun
{
    public MachineGun(PoolManager pool, Transform muzzle, GunData gunData) : base(pool, muzzle, gunData) { }

    public override void Shoot()
    {
        GetBullet();
    }
}

