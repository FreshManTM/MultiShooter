using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : Resource
{
    public override void Action(GameObject player)
    {
        player.GetComponentInChildren<PlayerController>().AddHealth(30);
    }
}
