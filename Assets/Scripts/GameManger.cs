using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PurrNet;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManger : NetworkBehaviour
{
    [Header("Players")]
    public PlayerLogic player1;
    public PlayerLogic player2;
    public PlayerLogic player3;
    public PlayerLogic player4;

    [Header("Player End Turn State")]
    public int nextTurnReady = 0;


    [Header("Scripts")]
    public SnakeDraft snakeDraft;
    public PlayerTypes playerTypes;
    public DiceRoll rolling;
    public CardServerManager cardServerManager;
    public CameraManager cameraManager;
    public CameraControl mainCamera; 
    public MainUI mainUI;
    public AI_Manager aiManager;

    //[SerializeField] private NetworkIdentity networkIdentity;
    

    [Header("Game Information")]
    public int Player1Score = 0;
    public int Player2Score = 0;
    public int Player3Score = 0;
    public int Player4Score = 0;
    public int turn = 0;
    public int numOfPlayers = 1;
    public int maxPlayers = 2;
    public SyncVar<int> currentPlayerTurn;
    public bool firstGame = true;

    

    public GameObject firstTile;
    public GameObject firstTileRound2;
    public TileParent tileParent;
    

    

    void Start()
    {
        //StartCoroutine(StartDraft());
        //FirstTurn();

    }


    //change turn to test
    
    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            cardServerManager.giveCardAllPlayers();
        }
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            cardServerManager.giveCardTargetPlayer(1);
        }

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            cardServerManager.giveCardTargetPlayer(2);
        }
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {

            FirstTurn();
            player1.SetUpCharacter(player1.character);
            player2.SetUpCharacter(player2.character);
            player3.SetUpCharacter(player3.character);
            player4.SetUpCharacter(player4.character);

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
            return player3;
        else if (playerNum == 4)
            return player4;
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
        return maxPlayers;

    }

    // returns player(s) in last place
    public List<PlayerLogic> getLastPlacePlayers()
    {

        PlayerLogic[] players = getAllPlayers();// append with more later
        int[] placements = new int[maxPlayers];

        for (int i = 0; i < maxPlayers; i++)
        {
            placements[i] = players[i].currentTile.gameObject.GetComponent<TileLogic>().id;
        }

        
    

        int minimum = placements.Min();

        List<PlayerLogic> result = new List<PlayerLogic>();

        for (int i = 0; i < maxPlayers; i++)
        {
            if (players[i].currentTile.gameObject.GetComponent<TileLogic>().id == minimum)
            {
                result.Add(players[i]); 
            }
        }

        return result;

    }

    // returns player in placement order
    public List<PlayerLogic> getPlayersInPlacementOrder()
    {

        PlayerLogic[] players = getAllPlayers();
        Array.Sort(players, (a, b) => a.CurrentTileId.CompareTo(b.CurrentTileId));
        return new List<PlayerLogic>(players);
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
        if (firstGame)
        {
            for (int i = 0; i < maxPlayers; i++)
            {
                if (i+1 > numOfPlayers)
                {
                    players[i].isAI = true;
                }
                players[i].CrossedFinish = false;
                players[i].TurnOffAllSprites();
                PlayerLogic.Character choice = players[i].pickedCharacters[0];
                players[i].character = choice;
                players[i].SetUpCharacter(choice);

            }
            Debug.Log("Setting up players Round 1");
        }
        else
        {
            for (int i = 0; i < maxPlayers; i++)
            {
                players[i].CrossedFinish = false;
                players[i].TurnOffAllSprites();
                players[i].StartTeleport(firstTileRound2); 
                PlayerLogic.Character choice = players[i].pickedCharacters[1];
                players[i].character = choice;
                players[i].SetUpCharacter(choice);

            }
            Debug.Log("Setting up players Round 2");
        }

    }

    


    //Begin draft, calls normal then reverse draft
    public IEnumerator StartDraft()
    {
           
        if (!isServer)
        {
            yield return null;
        }
        tileParent.SetAllTiles();

        currentPlayerTurn.value = 1;

        snakeDraft.showDraft(true);
        yield return StartCoroutine(snakeDraft.StartSnakeDraft());
        yield return StartCoroutine(snakeDraft.ReverseSnakeDraft());
        aiManager.NPCDraft(false);
        Debug.Log("Draft over");
        cameraManager.CameraSetUp();
       
        snakeDraft.showDraft(false);
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
        mainUI.UpdateNameUI();
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
        if (currentPlayerTurn > maxPlayers)
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

        if (cardServerManager.topsyTurvyOrgin == GetCurrentPlayer().PlayerId)
        {
            cardServerManager.topsyTurvy = false;
            cardServerManager.topsyInEffect(false);
        }
        if (cardServerManager.stickyWebOrigin == GetCurrentPlayer().PlayerId)
        {
            cardServerManager.stickyWeb = false;
            cardServerManager.stickyInEffect(false);
        }
        if (cardServerManager.bloodMoonOrgin == GetCurrentPlayer().PlayerId)
        {
            cardServerManager.bloodMoon = false;
            cardServerManager.bloodMoonInEffect(false);
        }
        if (cardServerManager.tauntOrgin == GetCurrentPlayer().PlayerId)
        {
            cardServerManager.taunt = false;
            cardServerManager.tauntInEffect(false);
        }
        Debug.Log("free cam");
        //cameraManager.LockToCurrentPlayer();

        if (GetCurrentPlayer().CrossedFinish == true)
        {

            if (CheckIfRaceEnd())
            {
                EndGame();
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
        if (GetCurrentPlayer().isAI == false)
        {
            playerTypes.CheckCharacterBeforeRole(GetCurrentPlayer());
            
        }
        else
        {
            StartCoroutine(aiManager.AI_Turn());
        }
        StartCoroutine(WaitForPlayers());
    }


    //ends turn, updates currentTurn number, starts next turn
    //also checks card effects states
    public void EndTurn()
    {
        //giga impact card, rest of logic in dice roll 
        if (cardServerManager.gigaImpact)
        {
            GetCurrentPlayer().skipTurn = true;
            cardServerManager.gigaImpact = false;
        }

        nextTurnReady = 0;
        
        if (currentPlayerTurn == maxPlayers)
        {



            turn++;
            Debug.Log("Round " + turn + " completed");
        }
        cardServerManager.clientCanPlayCards();
        NextTurn();
        StartTurn();


    }

    public void EndGame()
    {
        if (firstGame)
        {
            Debug.Log("First Round Over");
            firstGame = false;
            turn = 0;
            currentPlayerTurn.value = 0;
            playerSetUp(getAllPlayers());
            FirstTurn();


        }
        else
        {
            Debug.Log("Second Round Over");
        }


    }


    public bool CheckIfRaceEnd()
    {
        if(finishedPlayersAmount() == maxPlayers)
        {
            return true;
        }

        return false;

    }




    //updates score of player
    public void updateScore(PlayerLogic player, int points)
    {

        
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

        mainUI.UpdatePointUI();

    }



    

}
