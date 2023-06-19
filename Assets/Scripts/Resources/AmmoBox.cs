using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Resource
{
    public override void Action(GameObject player)
    {
        player.GetComponentInChildren<GunManager>().AddAmmo(10);
    }
}
