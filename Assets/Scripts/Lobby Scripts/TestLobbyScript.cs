using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine.InputSystem;
using Unity.Services.Lobbies;


//tutorial i used: https://www.youtube.com/watch?v=-KDlEBfCBiU&ab_channel=CodeMonkey
//im 11 min in



public class TestLobbyScript : MonoBehaviour
{

  

  

    private async void Start()
    {
        await UnityServices.InitializeAsync();


        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();




    }

    private void Update()
    {
        //testing 
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            CreateLobby();
        }
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            ListLobbies();
        }
    }


    private async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 4;


            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            Debug.Log("Created Lobby: " + lobby.Name + " " + lobby.MaxPlayers);
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private async void ListLobbies()
    {
        try
        {
            //CodeMonkey vid has await Lobbies.Instance.QueryLobbiesAsync(); (Depreciated)
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies Found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }

        }catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }



}
