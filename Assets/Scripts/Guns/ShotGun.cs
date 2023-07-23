using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ShotGun : Gun
{
    Vector3 _offset = new Vector3(0, 0, -90);
    Transform _muzzle;
    GunData _gunData;
    const float SPREAD = 8;
    public  ShotGun(PoolManager pool, Transform muzzle, GunData gunData): base(pool, muzzle, gunData)
    {
        _muzzle = muzzle;
        _gunData = gunData;
    }
    public override void Shoot()
    {
       
        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = GetBullet();
            float bulletOffset = (i - (3 / 2)) * SPREAD + _offset.z;
            bullet.transform.rotation = Quaternion.Euler(_muzzle.transform.eulerAngles.x, _muzzle.transform.eulerAngles.y, _muzzle.transform.eulerAngles.z + bulletOffset);
        
            bullet.GetComponent<Bullet>().Init(_gunData.Damage);
        }

    }
}

