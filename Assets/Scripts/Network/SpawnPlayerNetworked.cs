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
    [SerializeField] NetworkPlayer _playerPrefab;
    [SerializeField] GameObject _cameraPrefab;
    int _playersCount;
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        print("Player joined");
        _playersCount++;
        if(_playersCount == 2 && runner.IsSharedModeMasterClient)
        {
            FindObjectOfType<WaveController>().StartGameTimer();
        }

    }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (runner.Topology == SimulationConfig.Topologies.Shared)
        {
            print("OnSceneLoadDone. Local player spawn");
            GameObject playerObj = runner.Spawn(_playerPrefab, Vector2.zero, Quaternion.identity, runner.LocalPlayer).gameObject;

            GameObject cameraObj = Instantiate(_cameraPrefab);
            cameraObj.GetComponentInChildren<CinemachineVirtualCamera>().Follow = playerObj.transform;
            GetComponent<NetworkRunnerHandler>().LoadingCanvas.SetActive(false);
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        _playersCount--;
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
