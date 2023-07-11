using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

public class GunManager : NetworkBehaviour
{
    enum Platform{PC, ANDROID}
    [SerializeField] Platform platform;
   // [SerializeField] GunData[] gunData;
    GunData prevGunData;
    [SerializeField] GunData currentGunData;
    [SerializeField] Transform muzzle;
    [SerializeField] PoolManager pool;
    [SerializeField] Joystick shootJoystick;
    SpriteRenderer gunSprite;

    [SerializeField] int ammo;
    [SerializeField]float shotsTimer;
    
    bool flip;
    bool isFlipped;
    public GameManager gm;
    Vector3 offset = new Vector3(0, 0, -90);
    Gun gun;
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            StartCoroutine(FindScripts());
        }
    }
    IEnumerator FindScripts()
    {
        gunSprite = GetComponentInChildren<SpriteRenderer>();
        shootJoystick = GameObject.Find("Shooting Joystick").GetComponentInChildren<Joystick>();
        pool = FindObjectOfType<PoolManager>();
        gm = FindObjectOfType<GameManager>();
        if (gunSprite == null || shootJoystick == null || pool == null || gm == null)
        {
            yield return new WaitForSeconds(.1f);
            StartCoroutine(FindScripts());
        }
        else
        {
            gm.SetGun(this);

            yield return null;
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority)
            return;
        FollowGun(transform);
        Flip();
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
        }
        else
        {
            shotsTimer -= Time.fixedDeltaTime;
        }
    }

    public void InitGun(GunData data)
    {
        prevGunData = currentGunData;
        currentGunData = data;
        //gunSprite.sprite = currentGunData.GunSprite;
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
                            currentGunData = gm.GetGunData(0);
                            gun = new ShotGun(pool, muzzle, currentGunData);
                            gun.Shoot();
                            ammo -= 3;
                        }
                        break;
                    }
                case GunData.GunType.SawedOff:
                    {
                        currentGunData = gm.GetGunData(1);
                        gun = new SawedOffGun(pool, muzzle, currentGunData);
                        gun.Shoot();
                        ammo--;
                        break;
                    }
                case GunData.GunType.MachineGun:
                    {
                        currentGunData = gm.GetGunData(2);
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
        if (this.ammo > currentGunData.MagazineSize)
            this.ammo = currentGunData.MagazineSize;
    }
}