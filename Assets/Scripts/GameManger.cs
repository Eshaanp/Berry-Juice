using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using PurrNet;

public class GameManger : NetworkBehaviour
{
    [Header("Players")]
    public PlayerLogic player1;
    public PlayerLogic player2;

    [Header("Scripts")]
    
    public SnakeDraft snakeDraft;
    public PlayerTypes playerTypes;
    public DiceRoll rolling;
    

    [Header("Game Information")]
    public int Player1Score = 0;
    public int Player2Score = 0;
    public int turn = 0; 
    public int currentPlayerTurn = 1;
    public int numOfPlayers = 2;

    

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
            NextTurn();
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

        return (playerId == currentPlayerTurn);

    }

    //Gets the current player's playerLogic script 
    public PlayerLogic GetCurrentPlayer()
    {
        if (currentPlayerTurn == 1)
            return player1;
        else if (currentPlayerTurn == 2)
            return player2;
        else
            return null;
    }

    public PlayerLogic GetTargetPlayer(int playerNum)
    {
        if (playerNum == 1)
            return player1;
        else if (playerNum == 2)
            return player2;
        else
            return null;
    }


    public void playerSetUp()
    {
        PlayerLogic[] players = getAllPlayers();

        for (int i = 0; i < numOfPlayers; i++)
        {
            players[i].character = players[i].pickedCharacters[0];


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
        yield return StartCoroutine(snakeDraft.StartSnakeDraft());
        yield return StartCoroutine(snakeDraft.ReverseSnakeDraft());
        Debug.Log("Draft over");

        playerSetUp();
        FirstTurn();
    }

    //First turn, starts main game and sets initial variables
    public void FirstTurn()
    {
        //uiManager.gameObject.SetActive(true);
        currentPlayerTurn = 1;
        turn = 0;
        StartTurn();

    }

    //changes current player turn number
    public void NextTurn()
    {
        currentPlayerTurn = (currentPlayerTurn == 1) ? 2 : 1;
        Debug.Log("Player " + currentPlayerTurn + "'s turn");
        
    }


    //Starting turn, handle logic before Main dice roll
    //check if players turn should be skipped
    //checks the players character before role for effect (ie meowscarada)
    public void StartTurn()
    {
        
        if (GetCurrentPlayer().skipTurn == true)
        {
            GetCurrentPlayer().skipTurn = false;
            EndTurn();
            Debug.Log("skip");
            return;
            
        }
        playerTypes.CheckCharacterBeforeRole(GetCurrentPlayer());
        //StartCoroutine(GetCurrentPlayer().DiceRoll());

    }


    //ends turn, updates currentTurn number, starts next turn
    public void EndTurn()
    {

        if (currentPlayerTurn == numOfPlayers)
        {
            turn++;
            Debug.Log("Round " + turn + " completed");
        }

        NextTurn();
        StartTurn();


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

        } else if (player.PlayerId == 4)
        {

        }

    }

    //returns an array with all playerLogic scripts 
    public PlayerLogic[] getAllPlayers()
    {
        PlayerLogic[] maxPlayers = { player1, player2 }; // append with more later
        PlayerLogic[] result = new PlayerLogic[numOfPlayers];

        for (int i = 0; i < numOfPlayers; i++)
        { 
            result[i] = maxPlayers[i];
        }
        return result;

    }



}
