using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

public class GunManager : NetworkBehaviour
{
    enum Platform{PC, ANDROID}
    [SerializeField] Platform _platform;            //Platform from which the player plays

    GunData _prevGunData;                           //Data which should be assigned after player uses granade
    [SerializeField] GunData _currentGunData;       //Current data which player uses
    [SerializeField] Transform _muzzle;             //Transform from where the bullets are spawned
    [SerializeField] PoolManager _pool;             //Script which spawns bullets
    [SerializeField] Joystick _shootJoystick;       //Shooting joystick to get input from
    SpriteRenderer _gunSprite;                      //Sprite Renderer of the gun

    [SerializeField] int _ammo;                     //Current gun ammo               
    [SerializeField] float _shotsTimer;             //Timer which counts time between shoots
        
    bool _flip;                                     //Return true if the gun should be flipped
    bool _isFlipped;                                //Controls the object to be flipped only ones
    GameManager _gm;                                
    Vector3 _joystickOffset = new Vector3(0, 0, -90);
    Gun _gun;                                       //Type of the Gun
    float _rotZ;                                    //Counts the gun rotation

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            StartCoroutine(_FindScripts());
        }
    }
    IEnumerator _FindScripts()
    {
        _gunSprite = GetComponentInChildren<SpriteRenderer>();
        _shootJoystick = GameObject.Find("Shooting Joystick").GetComponentInChildren<Joystick>();
        _pool = FindObjectOfType<PoolManager>();
        _gm = FindObjectOfType<GameManager>();
        if (!_gunSprite || !_shootJoystick || !_pool || !_gm)
        {
            yield return new WaitForSeconds(.1f);
            StartCoroutine(_FindScripts());
        }
        else
        {
            _gm.SetGun(this);

            yield return null;
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority)
            return;

        FollowGun(transform);
        Flip();

        if(_shotsTimer <= 0)
        {
            if (_platform == Platform.PC && Input.GetKey(KeyCode.Mouse0))
            {
                Shoot();
                _shotsTimer = _currentGunData.TimeBetweenShots;
            }
            else if (_platform == Platform.ANDROID && _shootJoystick.Horizontal != 0 || _shootJoystick.Vertical != 0)
            {
                Shoot();
                _shotsTimer = _currentGunData.TimeBetweenShots;
            }
        }
        else
        {
            _shotsTimer -= Time.fixedDeltaTime;
        }
    }

    public void InitGun(GunData data)
    {
        _prevGunData = _currentGunData;
        _currentGunData = data;
        _gunSprite.sprite = _currentGunData.GunSprite;
        _ammo = _currentGunData.MagazineSize;
        _shotsTimer = 0;
        GetComponentInChildren<BoxCollider2D>().size = new Vector2(_currentGunData.Distance, _currentGunData.Distance);
    }

    private void Flip()
    {
        _flip = transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270;
        
        if (_flip && !_isFlipped)
        {
            _gunSprite.flipY = true;
            _isFlipped = true;
            _muzzle.transform.localPosition = new Vector2(_muzzle.transform.localPosition.x, _muzzle.transform.localPosition.y - 0.218f);
        }
        else if(!_flip && _isFlipped)
        {
            _gunSprite.flipY = false;
            _isFlipped = false;
            _muzzle.transform.localPosition = new Vector2(_muzzle.transform.localPosition.x, _muzzle.transform.localPosition.y + 0.218f);

        }
    }

    public virtual void FollowGun(Transform transform)
    {
        print(transform.parent.transform.eulerAngles.y);
        if (_platform == Platform.PC)
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (AllowRotation(difference))
            {
                _rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, _rotZ);
            }

        }
        else if (Mathf.Abs(_shootJoystick.Horizontal) > .01f)
        {

            if (AllowRotation(_shootJoystick.Direction))
            {
                _rotZ = Mathf.Atan2(_shootJoystick.Horizontal, -_shootJoystick.Vertical) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, _rotZ + _joystickOffset.z);
            }

        }
    }
    bool AllowRotation(Vector2 input)
    {
        if (transform.parent.transform.eulerAngles.y == 0)
        {
            if(input.x > 0)
            {
                return true;
            }
        }
        else
        {
            if (input.x < 0f)
            {
                return true;
            }
        }
        return false;
    }
    void Shoot()
    {
        if (_ammo > 0)
        {
            switch (_currentGunData.Type)
            {
                case GunData.GunType.ShotGun:
                    {
                        if (_ammo >= 3)
                        {
                            _currentGunData = _gm.GetGunData(0);
                            _gun = new ShotGun(_pool, _muzzle, _currentGunData);
                            _gun.Shoot();
                            _ammo -= 3;
                        }
                        break;
                    }
                case GunData.GunType.SawedOff:
                    {
                        _currentGunData = _gm.GetGunData(1);
                        _gun = new SawedOffGun(_pool, _muzzle, _currentGunData);
                        _gun.Shoot();
                        _ammo--;
                        break;
                    }
                case GunData.GunType.MachineGun:
                    {
                        _currentGunData = _gm.GetGunData(2);
                        _gun = new MachineGun(_pool, _muzzle, _currentGunData);
                        _gun.Shoot();
                        _ammo--;
                        break;
                    }
                case GunData.GunType.Grenade:
                    {
                        _gun = new GrenadeGun(_pool, _muzzle, _currentGunData);
                        _gun.Shoot();
                        _ammo--;
                        InitGun(_prevGunData);
                        break;
                    }
            }
        }
    }
    public int[] GetAmmo()
    {
        int[] magazine = new int[2];
        magazine[0] = _ammo;
        magazine[1] = _currentGunData.MagazineSize;
        return magazine;
    }
    public void AddAmmo(int ammo)
    {
        _ammo += ammo;
        if (_ammo > _currentGunData.MagazineSize)
            _ammo = _currentGunData.MagazineSize;
    }
}