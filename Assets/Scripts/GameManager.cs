using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController[] PlayerAnim;
    [SerializeField] RuntimeAnimatorController[] EnemyAnim;
    [SerializeField] GunData[] gunData;

    [SerializeField] Text killsText;
    [SerializeField] Text ammoText;
    [SerializeField] Slider healthSlider;

    public GunManager gunManager;
    [SerializeField] int kills;
    int[] ammo;

    public void SetGun(GunManager gun)
    {
        gunManager = gun;
    }
    public void Update()
    {
        SetText();
    }

    private void SetText()
    {
        if(gunManager != null)
        {
            ammo = gunManager.GetAmmo();
            ammoText.text = ammo[0] + "/" + ammo[1];
        }
        killsText.text = kills.ToString();
    }
    public void SetHealth(float health)
    {
        healthSlider.value = health/100;
    }
    public void AddKill()
    {
        kills++;
    }
    public void Death()
    {
        if(gunManager.GetComponent<NetworkObject>().HasInputAuthority)
            gunManager.gameObject.SetActive(false);

        Invoke(nameof(ChangeSpectator), 2);
    }
    void ChangeSpectator()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        GameObject player = null;
        foreach (var item in players)
        {
            if (!item.GetComponentInParent<NetworkObject>().HasInputAuthority)
                player = item.gameObject;
        }
        CinemachineVirtualCamera camera = FindObjectOfType<CinemachineVirtualCamera>();
        if (player != null)
            camera.Follow = player.transform;
        else
            print("GAME IS OVER!");
    }
    public RuntimeAnimatorController GetPlayerAnim(int index)
    {
        return PlayerAnim[index];
    }
    public RuntimeAnimatorController GetEnemyAnim(int index)
    {
        return EnemyAnim[index];
    }
    public GunData GetGunData(int index)
    {
        return gunData[index];
    }
}
