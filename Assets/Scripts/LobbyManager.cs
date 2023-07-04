using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Fusion;
using Fusion.Sockets;
using System.Linq;
using System.Threading.Tasks;
using System;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] InputField hostName;
    [SerializeField] GameObject loadingCanvas;
    NetworkRunner networkRunner;



    public void SetSkin(int skinNum)
    {
        PlayerPrefs.SetInt("PlayerSkin", skinNum);
    }
    private void Awake()
    {
        networkRunner = GetComponent<NetworkRunner>();
    }
    public void ConnectButton()
    {
        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Shared, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex +1, hostName.text, null);
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, string roomName, Action<NetworkRunner> initialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }
        loadingCanvas.SetActive(true);
        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = roomName,
            Initialized = initialized,
            SceneManager = sceneManager
        });
    }
}
