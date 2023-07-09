using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Cinemachine;

public enum GameState 
{
    Play,
    Win,
    Lose
}
public class GameManager : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController[] PlayerAnim;
    [SerializeField] RuntimeAnimatorController[] EnemyAnim;
    [SerializeField] GunData[] gunData;

    public GunManager gunManager;
    public int kills { get; set; }
    public float damage { get; set; }
    public float health { get; set; }
    public float playerDamage { get; set; }
    public GameState gameState { get; set; }
    
    public int[] ammo;
    [SerializeField] int playersAlive { get; set; }
    private void Awake()
    {
        gameState = GameState.Play;
        health = 100;
        playersAlive = 2;
    }
    public void SetGun(GunManager gun)
    {
        print("Setting gun in GM");
        gunManager = gun;
    }
    public void Death()
    {
        playersAlive--;
       
        gunManager.gameObject.SetActive(false);
        Invoke(nameof(ChangeSpectator), 2);
    }

    void ChangeSpectator()
    {
        if(FindObjectOfType<GunManager>()!= null)
        {
            GameObject player = FindObjectOfType<GunManager>().gameObject;
            CinemachineVirtualCamera camera = FindObjectOfType<CinemachineVirtualCamera>();
            print("Player is " + player);
            camera.Follow = player.transform;
        }
        else
        {
            print("GameState changed");
            gameState = GameState.Lose;
        }
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
