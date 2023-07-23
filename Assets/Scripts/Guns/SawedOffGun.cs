using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SawedOffGun: Gun
{
    public SawedOffGun(PoolManager pool, Transform muzzle, GunData gunData) : base(pool, muzzle, gunData) { }

    public override void Shoot()
    {
        GetBullet();
    }
}

