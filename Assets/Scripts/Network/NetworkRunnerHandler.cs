using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Linq;
using System;


public class NetworkRunnerHandler : MonoBehaviour
{

    NetworkRunner networkRunner;
    private void Awake()
    {
        networkRunner = GetComponent<NetworkRunner>();
    }
    void Start()
    {
        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Shared, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if(sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "Test Room",
            Initialized = initialized,
            SceneManager = sceneManager
        });
    }
}
