using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using Cinemachine;
using UnityEngine.UI;
public class SpawnPlayerNetworked : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPlayer playerPrefab;
    [SerializeField] GameObject cameraPrefab;
    [SerializeField] GameObject loadingCanvas;
    public void OnConnectedToServer(NetworkRunner runner)
    {
        if(runner.Topology == SimulationConfig.Topologies.Shared)
        {
            print("OnconnectedToServer. Local player");
            GameObject playerObj = runner.Spawn(playerPrefab, Vector2.zero, Quaternion.identity, runner.LocalPlayer).gameObject;

            GameObject cameraObj = Instantiate(cameraPrefab);
            cameraObj.GetComponentInChildren<CinemachineVirtualCamera>().Follow = playerObj.transform;
            loadingCanvas.SetActive(false);
        }
        else
        {
            print("OnConnectedToServer");
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            print("OnPlayerJoined. Spawning player");
            runner.Spawn(playerPrefab, Vector2.zero, Quaternion.identity, player);
        }
        else
        {
            print("OnPlayerJoined");
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }


    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }



    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        print("OnSceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        print("OnSceneLoadStart");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}
