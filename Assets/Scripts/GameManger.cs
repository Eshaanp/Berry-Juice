using UnityEngine;
using UnityEngine.InputSystem;

public class GameManger : MonoBehaviour
{

    public PlayerLogic player1;
    public PlayerLogic player2;



    public int currentPlayerTurn = 1;

    void Start()
    {

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

    
    public void NextTurn()
    {
        currentPlayerTurn = (currentPlayerTurn == 1) ? 2 : 1;
        Debug.Log("Player " + currentPlayerTurn + "'s turn");
    }






}
