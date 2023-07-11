using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using Cinemachine;
using UnityEngine.UI;
public class SpawnPlayerNetworked : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPlayer playerPrefab;
    [SerializeField] GameObject cameraPrefab;
    int playersCount;
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        print("Player joined");
        playersCount++;
        if(playersCount == 2 && runner.IsSharedModeMasterClient)
        {
            FindObjectOfType<WaveController>().StartGameTimer();
        }

    }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (runner.Topology == SimulationConfig.Topologies.Shared)
        {
            print("OnSceneLoadDone. Local player spawn");
            GameObject playerObj = runner.Spawn(playerPrefab, Vector2.zero, Quaternion.identity, runner.LocalPlayer).gameObject;

            GameObject cameraObj = Instantiate(cameraPrefab);
            cameraObj.GetComponentInChildren<CinemachineVirtualCamera>().Follow = playerObj.transform;
            GetComponent<NetworkRunnerHandler>().loadingCanvas.SetActive(false);
            //Runner.SetPlayerObject(playerObj.GetComponent<NetworkObject>().InputAuthority, playerObj.GetComponent<NetworkObject>());
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        playersCount--;
    }

    public void OnInput(NetworkRunner runner, NetworkInput input){}
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}
    public void OnDisconnectedFromServer(NetworkRunner runner){}
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data){}
    public void OnSceneLoadStart(NetworkRunner runner){}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){}
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){}
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}

}
