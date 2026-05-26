using PurrNet;
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
        // Disable so it doesn't fire multiple times
        enabled = false;
        Debug.Log("START");

        // PurrNet's server-side scene load — all clients follow automatically
        SceneManager.LoadSceneAsync(nextScene);
    }
}