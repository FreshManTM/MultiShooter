using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public abstract class Gun : NetworkBehaviour
{
    Vector3 _offset = new Vector3(0, 0, -90);
    PoolManager _pool;
    Transform _muzzle; 
    GunData _gunData;

    public Gun(PoolManager pool, Transform muzzle, GunData gunData)
    {
        _pool = pool;
        _muzzle = muzzle;
        _gunData = gunData;
    }
    public abstract void Shoot();
    protected GameObject GetBullet()
    {
        GameObject bullet = _pool.SpawnResource(0);
        bullet.transform.position = _muzzle.position;
        bullet.transform.rotation = Quaternion.Euler(_muzzle.transform.rotation.eulerAngles + _offset);

        bullet.GetComponent<Bullet>().Init(_gunData.Damage);
        return bullet;
    }
}
