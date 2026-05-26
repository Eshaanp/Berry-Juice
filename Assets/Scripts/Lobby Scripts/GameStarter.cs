using PurrNet;
using PurrNet.Modules;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [PurrScene, SerializeField] private string nextScene;
    [SerializeField] private int requiredPlayers = 2;

    private void Update()
    {
        if (!networkManager.isHost) return;
        if (networkManager.playerCount >= requiredPlayers)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        enabled = false;
        Debug.Log("START");

        var settings = new PurrSceneSettings
        {
            isPublic = true,
            mode = LoadSceneMode.Single
        };

        networkManager.sceneModule.LoadSceneAsync(nextScene, settings);
    }
}