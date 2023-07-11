using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }
    public Animator playerAnim;

    GameManager gm;
    GunManager gunManager;
    [Networked(OnChanged =nameof(OnSkinChanged))] int skinNumber { get; set; }
    [Networked(OnChanged =nameof(OnSkinChanged))]public int gunNumber { get; set; }

    static void OnSkinChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnSpriteChanged();
    }

    void OnSpriteChanged()
    {
        gm = FindObjectOfType<GameManager>();
        gunManager = GetComponentInChildren<GunManager>();

        playerAnim.runtimeAnimatorController = gm.GetPlayerAnim(skinNumber);

        gunManager.GetComponentInChildren<SpriteRenderer>().sprite = gm.GetGunData(gunNumber).GunSprite;
        gunManager.InitGun(gm.GetGunData(gunNumber));
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetSprite(int skinNumber, int gunNumber, RpcInfo info = default)
    {
        this.skinNumber = skinNumber;
        this.gunNumber = gunNumber;
    }
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;

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
