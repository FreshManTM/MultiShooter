using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] Text killsText;
    [SerializeField] Text ammoText;
    [SerializeField] GunManager gunManager;

    [SerializeField] int kills;
    int[] ammo;
    private void Start()
    {
    }
    private void Update()
    {
        ammo = gunManager.GetAmmo();
        killsText.text = kills.ToString();
        ammoText.text = ammo[0] + "/" + ammo[1];
    }

    public void AddKill()
    {
        kills++;
    }
}
