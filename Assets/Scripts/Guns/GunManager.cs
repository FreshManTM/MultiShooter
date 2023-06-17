using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    enum Platform{PC, ANDROID}
    [SerializeField] Platform platform;
    [SerializeField] GunData gunData;
    [SerializeField] Transform muzzle;
    [SerializeField] PoolManager pool;
    [SerializeField] Joystick shootJoystick;
    SpriteRenderer gunSprite;

    [SerializeField] int ammo;
    float shotsTimer;
    float shotgunSpread = 8;
    
    bool reloading;
    bool shooting;
    bool flip;
    bool isFlipped;
    
    Vector3 offset = new Vector3(0, 0, -90);

    void Start()
    {
        gunSprite = GetComponent<SpriteRenderer>();
        gunSprite.sprite = gunData.GunSprite;
        GetComponentInChildren<BoxCollider2D>().size = new Vector2(gunData.Distance, gunData.Distance);
        ammo = gunData.MagazineSize;
    }
    void Update()
    {
        FollowGun(transform);
        Flip();
        if (ammo < gunData.MagazineSize && !reloading && !shooting)
        {
            StartCoroutine(Reload());
        }
        if(shotsTimer <= 0)
        {
            if (platform == Platform.PC && Input.GetKey(KeyCode.Mouse0))
            {
                Shoot();
                shotsTimer = gunData.TimeBetweenShots;
            }
            else if (platform == Platform.ANDROID && shootJoystick.Horizontal != 0 || shootJoystick.Vertical != 0)
            {
                Shoot();
                shotsTimer = gunData.TimeBetweenShots;
            }
            else
            {
                shooting = false;

            }
        }
        else
        {
            shotsTimer -= Time.deltaTime;
        }
    }

    private void Flip()
    {
        flip = transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270;
        
        if (flip && !isFlipped)
        {
            gunSprite.flipY = true;
            isFlipped = true;
            muzzle.transform.localPosition = new Vector2(muzzle.transform.localPosition.x, muzzle.transform.localPosition.y - 0.218f);
        }
        else if(!flip && isFlipped)
        {
            gunSprite.flipY = false;
            isFlipped = false;
            muzzle.transform.localPosition = new Vector2(muzzle.transform.localPosition.x, muzzle.transform.localPosition.y + 0.218f);

        }
    }

    public virtual void FollowGun(Transform transform)
    {
        float rotZ;
        if (platform == Platform.PC)
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
        else if (platform == Platform.ANDROID && Mathf.Abs(shootJoystick.Horizontal) > .01f)
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
            if(ammo >= 3 && !reloading)
            {
                shooting = true;
                for (int i = 0; i < 3; i++)
                {
                    GameObject bullet = Get();
                    float bulletOffset = (i - (3 / 2)) * shotgunSpread + offset.z;
                    bullet.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + bulletOffset);
                }       
                ammo -= 3;
            }
            else
            {
                shooting = false;
                return;
            }
        }
        else
        {
            if (ammo > 0 && !reloading)
            {
                shooting = true;
                Get();
                ammo--;
            }
            else
            {
                shooting = false;
                return;
            }
        }
        
    }
    IEnumerator Reload()
    {
        if(ammo < gunData.MagazineSize)
        {
            reloading = true;
            ammo++;
            yield return new WaitForSeconds(gunData.ReloadTime / gunData.MagazineSize);
            StartCoroutine(Reload());
        }
        else
        {
            reloading = false;
            yield return null;
        }

    }
    public int[] GetAmmo()
    {
        int[] magazine = new int[2];
        magazine[0] = ammo;
        magazine[1] = gunData.MagazineSize;
        return magazine;
    }
    public void AddAmmo(int ammo)
    {
        this.ammo += ammo;
    }
}