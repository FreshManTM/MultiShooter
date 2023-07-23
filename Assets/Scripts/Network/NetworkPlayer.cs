using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    [SerializeField] Animator _playerAnim;

    GameManager _gm;
    GunManager _gunManager;
    [Networked(OnChanged =nameof(OnSkinChanged))] int _skinNumber { get; set; }
    [Networked(OnChanged =nameof(OnSkinChanged))] int _gunNumber { get; set; }

    static void OnSkinChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnSpriteChanged();
    }

    void OnSpriteChanged()
    {
        _gm = FindObjectOfType<GameManager>();
        _gunManager = GetComponentInChildren<GunManager>();
        
        _playerAnim.runtimeAnimatorController = _gm.GetPlayerAnim(_skinNumber);

        _gunManager.GetComponentInChildren<SpriteRenderer>().sprite = _gm.GetGunData(_gunNumber).GunSprite;
        _gunManager.InitGun(_gm.GetGunData(_gunNumber));
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetSprite(int skinNumber, int gunNumber, RpcInfo info = default)
    {
        _skinNumber = skinNumber;
        _gunNumber = gunNumber;
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
             RPC_SetSprite(PlayerPrefs.GetInt("PlayerSkin"), Random.Range(0, 3));   
        }
        else
        {
            print("Spawned Remote player");
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if(player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }
}
