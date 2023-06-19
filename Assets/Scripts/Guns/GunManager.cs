using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    enum Platform{PC, ANDROID}
    [SerializeField] Platform platform;
    [SerializeField] GunData[] gunData;
    GunData prevGunData;
    [SerializeField] GunData currentGunData;
    [SerializeField] Transform muzzle;
    [SerializeField] PoolManager pool;
    [SerializeField] Joystick shootJoystick;
    SpriteRenderer gunSprite;

    [SerializeField] int ammo;
    [SerializeField]float shotsTimer;
    
    bool reloading;
    bool shooting;
    bool flip;
    bool isFlipped;
    
    Vector3 offset = new Vector3(0, 0, -90);
    Gun gun;
    void Awake()
    {
        gunSprite = GetComponent<SpriteRenderer>();
        InitGun(gunData[0]);
    }
    void Update()
    {
        FollowGun(transform);
        Flip();
        print(currentGunData);
        if(shotsTimer <= 0)
        {
            if (platform == Platform.PC && Input.GetKey(KeyCode.Mouse0))
            {
                Shoot();
                shotsTimer = currentGunData.TimeBetweenShots;
            }
            else if (platform == Platform.ANDROID && shootJoystick.Horizontal != 0 || shootJoystick.Vertical != 0)
            {
                Shoot();
                shotsTimer = currentGunData.TimeBetweenShots;
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

    public void InitGun(GunData data)
    {
        prevGunData = currentGunData;
        currentGunData = data;
        gunSprite.sprite = currentGunData.GunSprite;
        ammo = currentGunData.MagazineSize;
        shotsTimer = 0;
        GetComponentInChildren<BoxCollider2D>().size = new Vector2(currentGunData.Distance, currentGunData.Distance);
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
    void Shoot()
    {
        if (ammo > 0)
        {
            switch (currentGunData.Type)
            {
                case GunData.GunType.ShotGun:
                    {
                        if (ammo >= 3)
                        {
                            currentGunData = gunData[0];
                            gun = new ShotGun(pool, muzzle, currentGunData);
                            gun.Shoot();
                            ammo -= 3;
                        }
                        break;
                    }
                case GunData.GunType.SawedOff:
                    {
                        currentGunData = gunData[1];
                        gun = new SawedOffGun(pool, muzzle, currentGunData);
                        gun.Shoot();
                        ammo--;
                        break;
                    }
                case GunData.GunType.MachineGun:
                    {
                        currentGunData = gunData[2];
                        gun = new MachineGun(pool, muzzle, currentGunData);
                        gun.Shoot();
                        ammo--;
                        break;
                    }
                case GunData.GunType.Grenade:
                    {
                        gun = new GrenadeGun(pool, muzzle, currentGunData);
                        gun.Shoot();
                        ammo--;
                        InitGun(prevGunData);
                        break;
                    }
            }
        }
    }
    IEnumerator Reload()
    {
        if(ammo < currentGunData.MagazineSize)
        {
            reloading = true;
            ammo++;
            yield return new WaitForSeconds(currentGunData.ReloadTime / currentGunData.MagazineSize);
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
        magazine[1] = currentGunData.MagazineSize;
        return magazine;
    }
    public void AddAmmo(int ammo)
    {
        this.ammo += ammo;
    }
}