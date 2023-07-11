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
public class GameManager : NetworkBehaviour
{
    [SerializeField] RuntimeAnimatorController[] PlayerAnim;
    [SerializeField] RuntimeAnimatorController[] EnemyAnim;
    [SerializeField] GunData[] gunData;

    public int kills { get; set; }
    public float damage { get; set; }
    public float health { get; set; }
    GunManager gunManager;
    [Networked] public GameState gameState { get; set; }
    
    public int[] ammo;
    public override void Spawned()
    {
        RPC_SetState(GameState.Play);
        health = 100;
    }
    public void SetGun(GunManager gun)
    {
        print("Setting gun in GM");
        gunManager = gun;
    }
    public void Death()
    {
       
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
            RPC_SetState(GameState.Lose);
        }
    }

    [Rpc]
    public void RPC_SetState(GameState state, RpcInfo info = default)
    {
        gameState = state;
    }
    public RuntimeAnimatorController GetPlayerAnim(int index) { return PlayerAnim[index]; }
    public RuntimeAnimatorController GetEnemyAnim(int index) { return EnemyAnim[index]; }
    public GunData GetGunData(int index){ return gunData[index]; }
}
