using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefabs;
    [SerializeField] GameObject[] BulletPrefab;

    List<GameObject> enemyPool;
    List<GameObject>[] bulletPool;

    void Awake()
    {
        bulletPool = new List<GameObject>[BulletPrefab.Length];
        enemyPool = new List<GameObject>();

        for (int i = 0; i < bulletPool.Length; i++)
        {
            bulletPool[i] = new List<GameObject>();
        }
    }

    public GameObject GetEnemy()
    {
        GameObject select = null;

        foreach (GameObject item in enemyPool) {
            if (!item.activeSelf) {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (!select) {
            select = Instantiate(EnemyPrefabs, transform);
            enemyPool.Add(select);
        }

        return select;
    }
    public GameObject GetBullet(int index)
    {
        GameObject select = null;
        foreach (GameObject item in bulletPool[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                print("get " + select.name);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(BulletPrefab[index], transform);
            bulletPool[index].Add(select);
        }

        return select;
    }
}

