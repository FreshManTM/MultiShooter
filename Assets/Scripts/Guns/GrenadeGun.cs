using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeGun : Gun
{
    Vector3 _offset = new Vector3(0, 0, -90);
    PoolManager _pool;
    Transform _muzzle;
    GunData _gunData;
    public GrenadeGun(PoolManager pool, Transform muzzle, GunData gunData) : base(pool, muzzle, gunData)
    {
        _pool = pool;
        _muzzle = muzzle;
        _gunData = gunData;
    }
    public override void Shoot()
    {
        GameObject bullet = _pool.SpawnResource(1);
        bullet.transform.position = _muzzle.position;
        bullet.transform.rotation = Quaternion.Euler(_muzzle.transform.rotation.eulerAngles + _offset);
        bullet.GetComponent<GrenadeBullet>().Init(_gunData.Damage);
    }
}
