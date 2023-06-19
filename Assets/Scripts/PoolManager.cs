using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Prefabs MUST be assigned in the following order:
    0) Enemy
    1) Player Bullet
    2) Skeleton Bullet
    3) Grenade
    4) Medkit
    5) Ammo Boxes
 */
public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject[] Prefabs;

    List<GameObject>[] prefabPool;

    void Awake()
    {
        prefabPool = new List<GameObject>[Prefabs.Length];

        for (int i = 0; i < prefabPool.Length; i++)
        {
            prefabPool[i] = new List<GameObject>();
        }
    }
    public GameObject Get(int index)
    {
        GameObject select = null;
        foreach (GameObject item in prefabPool[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(Prefabs[index], transform);
            prefabPool[index].Add(select);
        }

        return select;
    }
}

