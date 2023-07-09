using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Linq;
using System;
using UnityEngine.UI;


public class NetworkRunnerHandler : MonoBehaviour
{
    public GameObject loadingCanvas;
    [SerializeField] InputField roomName;
    NetworkRunner networkRunner;
    private void Awake()
    {
        NetworkRunner networkRunnerInScene = FindObjectOfType<NetworkRunner>();
        networkRunner = GetComponent<NetworkRunner>();
        if (networkRunnerInScene != null && networkRunnerInScene != networkRunner)
        {
            Destroy(networkRunner);
            networkRunner = networkRunnerInScene;
        }
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, string sessionName, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = sessionName,
            PlayerCount = 2,
            Initialized = initialized,
            SceneManager = sceneManager
        });
    }

    public void CreateJoinGame()
    {
        loadingCanvas.SetActive(true);
        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Shared, roomName.text, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex + 1, null);
    }
    public void SetPlayerSkin(int skinNum)
    {
        PlayerPrefs.SetInt("PlayerSkin", skinNum);
    }
}
