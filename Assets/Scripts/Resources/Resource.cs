using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public abstract class Resource : NetworkBehaviour
{
    public abstract void Action(GameObject player);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Action(collision.gameObject);
            if(Runner.IsSharedModeMasterClient)
                Runner.Despawn(Object);

        }
    }
}
