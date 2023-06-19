using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Resource
{
    [SerializeField] GunData grenadeData;
    public override void Action(GameObject player)
    {
        player.GetComponentInChildren<GunManager>().InitGun(grenadeData);
        gameObject.SetActive(false);

    }
}
