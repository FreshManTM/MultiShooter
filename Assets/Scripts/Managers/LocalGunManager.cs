using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGunManager : MonoBehaviour
{
    enum Platform { PC, ANDROID }
    float _rotZ;

    [SerializeField] Platform _platform;
    [SerializeField] Joystick _shootJoystick;
    Vector3 _offset = new Vector3(0, 0, -90);

    private void FixedUpdate()
    {
        FollowGun(transform);
    }

    public virtual void FollowGun(Transform transform)
    {
        print(transform.eulerAngles);

            if (_platform == Platform.PC)
            {
                Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                _rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, _rotZ);
            }
            else if (_platform == Platform.ANDROID && Mathf.Abs(_shootJoystick.Horizontal) > .01f)
            {
                _rotZ = Mathf.Atan2(_shootJoystick.Horizontal, -_shootJoystick.Vertical) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, _rotZ + _offset.z);
            }
        

    }
}
