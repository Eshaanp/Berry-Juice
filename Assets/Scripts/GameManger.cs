using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManger : MonoBehaviour
{

    public PlayerLogic player1;
    public PlayerLogic player2;

    public int Player1Score = 0;
    public int Player2Score = 0;

    public int turn = 0; 
    public int currentPlayerTurn = 1;
    public int numOfPlayers = 2;

    public PlayerTypes playerTypes;

    void Start()
    {
        StartTurn();
        numOfPlayers = 2;
    }

    
    //change turn to test
    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            NextTurn();
        }
    }

    //returns current players turn number
    public bool isPlayersTurn(int playerId) {

        return (playerId == currentPlayerTurn);

    }
    public PlayerLogic GetCurrentPlayer()
    {
        if (currentPlayerTurn == 1)
            return player1;
        else
            return player2;
    }

    public void NextTurn()
    {
        currentPlayerTurn = (currentPlayerTurn == 1) ? 2 : 1;
        Debug.Log("Player " + currentPlayerTurn + "'s turn");
        StartTurn();
    }


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



    public void EndTurn()
    {

        if (currentPlayerTurn == numOfPlayers)
        {
            turn++;
            Debug.Log("Round " + turn + " completed");
        }

        NextTurn();
        


    }


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

}
