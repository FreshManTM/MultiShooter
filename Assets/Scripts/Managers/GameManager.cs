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
    [SerializeField] RuntimeAnimatorController[] _playerAnim;
    [SerializeField] RuntimeAnimatorController[] _enemyAnim;
    [SerializeField] GunData[] _gunData;

    public int Kills { get; set; }
    public float Damage { get; set; }
    public float Health { get; set; }
    [Networked] public GameState GameState { get; set; }
    
    public int[] Ammo;
    GunManager _gunManager;
    public override void Spawned()
    {
        RPC_SetState(GameState.Play);
        Health = 100;
    }
    public override void FixedUpdateNetwork()
    {
        if (_gunManager != null)
            Ammo = _gunManager.GetAmmo();
    }
    public void SetGun(GunManager gun)
    {
        _gunManager = gun;
    }
    public void Death()
    {
        _gunManager.gameObject.SetActive(false);
        Invoke(nameof(ChangeSpectator), 2);
    }

    void ChangeSpectator()
    {
        if(FindObjectOfType<GunManager>())
        {
            GameObject player = FindObjectOfType<GunManager>().gameObject;
            CinemachineVirtualCamera camera = FindObjectOfType<CinemachineVirtualCamera>();
            camera.Follow = player.transform;
        }
        else
        {
            RPC_SetState(GameState.Lose);
        }
    }

    [Rpc]
    public void RPC_SetState(GameState state, RpcInfo info = default)
    {
        GameState = state;
    }
    public RuntimeAnimatorController GetPlayerAnim(int index) { return _playerAnim[index]; }
    public RuntimeAnimatorController GetEnemyAnim(int index) { return _enemyAnim[index]; }
    public GunData GetGunData(int index){ return _gunData[index]; }
}
