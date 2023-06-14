using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{

    [SerializeField] GunData gunData;
    [SerializeField] Transform muzzle;
    [SerializeField] PoolManager pool;

    SpriteRenderer gunSprite;
    Vector3 offset = new Vector3(0, 0, -90);
    float shotsTimer;

    float shotgunSpread = 8;
    [SerializeField] Joystick shootJoystick;
    public bool PCVersion;
    void Start()
    {
        gunSprite = GetComponent<SpriteRenderer>();
        GetComponentInChildren<BoxCollider2D>().size = new Vector2(gunData.Distance, gunData.Distance);

    }
    void Update()
    {
        FollowGun(transform);
        Flip();
        if(shotsTimer <= 0)
        {
            if (PCVersion && Input.GetKey(KeyCode.Mouse0))
            {
                //Get();
                Shoot();
                shotsTimer = gunData.TimeBetweenShots;
            }
            else if(!PCVersion && shootJoystick.Horizontal != 0 || shootJoystick.Vertical != 0)
            {
                Shoot();
                shotsTimer = gunData.TimeBetweenShots;
            }
        }
        else
        {
            shotsTimer -= Time.deltaTime;
        }
    }

    private void Flip()
    {
        if (transform.rotation.z < -0.7 || transform.rotation.z > 0.7)
        {
            gunSprite.flipY = true;
        }
        else
        {
            gunSprite.flipY = false;
        }
    }

    public virtual void FollowGun(Transform transform)
    {
        float rotZ;
        if (PCVersion)
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
        else if(!PCVersion && Mathf.Abs(shootJoystick.Horizontal) > .01f)
        {
            rotZ = Mathf.Atan2(shootJoystick.Horizontal, -shootJoystick.Vertical) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset.z);
        }
    }
    GameObject Get()
    {
        GameObject bullet = pool.GetBullet(0);
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + offset);

        bullet.GetComponent<Bullet>().Init(gunData.Damage);
        return bullet;
    }
    void Shoot()
    {

        if (gunData.Type == GunData.GunType.ShotGun)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject bullet = Get();
                float bulletOffset = (i - (3 / 2)) * shotgunSpread;
                bullet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                bullet.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + bulletOffset + offset.z);
            }       
        }
        else
        {
            Get();
        }
    }
}

