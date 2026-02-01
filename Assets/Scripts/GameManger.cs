using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using PurrNet;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

public class GameManger : NetworkBehaviour
{
    [Header("Players")]
    public PlayerLogic player1;
    public PlayerLogic player2;
    public PlayerLogic player3;
    public PlayerLogic player4;

    [Header("Player End Turn State")]
    public bool player1Ready = false;
    public bool player2Ready = false;
    public bool player3Ready = false;
    public bool player4Ready = false;
    public int nextTurnReady = 0;


    [Header("Scripts")]
    public SnakeDraft snakeDraft;
    public PlayerTypes playerTypes;
    public DiceRoll rolling;

    //[SerializeField] private NetworkIdentity networkIdentity;
    

    [Header("Game Information")]
    public int Player1Score = 0;
    public int Player2Score = 0;
    public int Player3Score = 0;
    public int Player4Score = 0;
    public int turn = 0;
    public int numOfPlayers = 2;
    public SyncVar<int> currentPlayerTurn;
    

    public GameObject firstTile;


    

    void Start()
    {
        //StartCoroutine(StartDraft());
        //FirstTurn();
    }


    //change turn to test
    
    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            
        }
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {

            FirstTurn();
            
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {

            StartCoroutine(StartDraft());
            
        }
    }


    //checks if a players turn
    public bool isPlayersTurn(int playerId) {

        return (playerId == currentPlayerTurn.value);

    }

    //Gets the current player's playerLogic script 
    public PlayerLogic GetCurrentPlayer()
    {
        if (currentPlayerTurn == 1)
            return player1;
        else if (currentPlayerTurn == 2)
            return player2;
        else if (currentPlayerTurn == 3)
            return player3;
        else if (currentPlayerTurn == 4)
            return player4;
        else
            return null;
    }

    public PlayerLogic GetTargetPlayer(int playerNum)
    {
        if (playerNum == 1)
            return player1;
        else if (playerNum == 2)
            return player2;
        else if (playerNum == 3)
            return player2;
        else if (playerNum == 4)
            return player2;
        else
            return null;
    }

    //returns an array with all playerLogic scripts 
    public PlayerLogic[] getAllPlayers()
    {
        PlayerLogic[] maxPlayers = { player1, player2 , player3, player4}; // append with more later
        PlayerLogic[] result = new PlayerLogic[numOfPlayers];

        for (int i = 0; i < numOfPlayers; i++)
        {
            result[i] = maxPlayers[i];
        }
        return result;

    }

    // returns player(s) in last place
    public List<PlayerLogic> getLastPlacePlayers()
    {

        PlayerLogic[] players = getAllPlayers();// append with more later
        int[] placements = new int[numOfPlayers];

        for (int i = 0; i < numOfPlayers; i++)
        {
            placements[i] = players[i].currentTile.gameObject.GetComponent<TileLogic>().id;
        }

        
    

        int minimum = placements.Min();

        List<PlayerLogic> result = new List<PlayerLogic>();

        for (int i = 0; i < numOfPlayers; i++)
        {
            if (players[i].currentTile.gameObject.GetComponent<TileLogic>().id == minimum)
            {
                result.Add(players[i]); 
            }
        }

        return result;

    }


    public int finishedPlayersAmount()
    {
        PlayerLogic[] maxPlayers = getAllPlayers(); // append with more later

        int result = 0;

        for (int i = 0; i < maxPlayers.Length; i++)
        {
            if (maxPlayers[i].currentTile.gameObject.GetComponent<TileLogic>().tileType == TileLogic.TileType.EndTile)
            {
                result++;
            }
        }
        return result;

    }

    
    public void playerSetUp(PlayerLogic[] players)
    {
        //PlayerLogic[] players = getAllPlayers();

        for (int i = 0; i < numOfPlayers; i++)
        {
            PlayerLogic.Character choice = players[i].pickedCharacters[0];
            players[i].character = choice;
            players[i].SetUpCharacter(choice);

        }
        Debug.Log("Setting up players");
    }

    


    //Begin draft, calls normal then reverse draft
    public IEnumerator StartDraft()
    {
        if (!isServer)
        {
            yield return null;
        }
        currentPlayerTurn.value = 1;

        yield return StartCoroutine(snakeDraft.StartSnakeDraft());
        yield return StartCoroutine(snakeDraft.ReverseSnakeDraft());
        Debug.Log("Draft over");

        playerSetUp(getAllPlayers());
        FirstTurn();
    }

    [ServerRpc]
    public void playerReady(RPCInfo info = default)
    {

        ushort id = (ushort)info.sender.id;
        Debug.Log("Player Ready: " + id);
        nextTurnReady += 1;

    }



    public IEnumerator WaitForPlayers()
    {

        yield return new WaitUntil(() => nextTurnReady == numOfPlayers);

        EndTurn();
    }




    //First turn, starts main game and sets initial variables
    public void FirstTurn()
    {
        //uiManager.gameObject.SetActive(true);
        currentPlayerTurn.value = 1;
        turn = 0;
        StartTurn();

    }

    //changes current player turn number
    public void NextTurn()      
    {
        //currentPlayerTurn = (currentPlayerTurn == 1) ? 2 : 1;
        currentPlayerTurn.value++;
        if (currentPlayerTurn > numOfPlayers)
        {
            currentPlayerTurn.value = 1;
        }

        Debug.Log("Player " + currentPlayerTurn + "'s turn");
        
    }


    //Starting turn, handle logic before Main dice roll
    //check if players turn should be skipped
    //checks the players character before role for effect (ie meowscarada)
    public void StartTurn()
    {
        if (GetCurrentPlayer().CrossedFinish == true)
        {

            if (CheckIfRaceEnd())
            {
                EndGame();
                Debug.Log("Game Ended");
                return;
            }    
            EndTurn();
            Debug.Log("crossed Finish");
            return;

        }
        else if (GetCurrentPlayer().skipTurn == true)
        {
            GetCurrentPlayer().skipTurn = false;
            EndTurn();
            Debug.Log("skip");
            return;
            
        }

        playerTypes.CheckCharacterBeforeRole(GetCurrentPlayer());
        StartCoroutine(WaitForPlayers());
        //StartCoroutine(GetCurrentPlayer().DiceRoll());

    }


    //ends turn, updates currentTurn number, starts next turn
    public void EndTurn()
    {
        nextTurnReady = 0;

        if (currentPlayerTurn == numOfPlayers)
        {
            turn++;
            Debug.Log("Round " + turn + " completed");
        }

        NextTurn();
        StartTurn();


    }

    public void EndGame()
    {

        Debug.Log("Game Ended");


    }


    public bool CheckIfRaceEnd()
    {
        if(finishedPlayersAmount() == numOfPlayers)
        {
            return true;
        }

        return false;

    }




    //updates score of player
    public void updateScore(int points)
    {

        PlayerLogic player = GetCurrentPlayer();

        
        if (player.PlayerId == 1)
        {
            Player1Score += points;
        } else if (player.PlayerId == 2)
        {
            Player2Score += points;
        } else if (player.PlayerId == 3)
        {
            Player3Score += points;
        } else if (player.PlayerId == 4)
        {
            Player4Score += points;
        }

    }



    

}
