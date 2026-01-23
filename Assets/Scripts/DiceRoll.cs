using PurrNet;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DiceRoll : NetworkBehaviour
{

    public GameManger gameManger;
    public PlayerTypes characterType;

    public bool isDicePressed = false;
    public Button diceButton;
    public int moveNum = 1;


    private void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            
            //StartCoroutine(diceRoll());
        }
    }

    public IEnumerator diceRoll() 
    {
       
        
        if (!isServer)
        {
            yield break;
        }
        

        isDicePressed = false;
        ShowDiceUI();

        while (!isDicePressed)
        {
            yield return null;
        }
        isDicePressed = true;
        HideDiceUI();
       // Debug.Log("Dice rolled a 2");


        if(gameManger.GetCurrentPlayer().character == PlayerLogic.Character.Meowscarada)
        {
            characterType.ReRoll(gameManger.GetCurrentPlayer(), DiceRollNumber());
        }

        else if (gameManger.GetCurrentPlayer().character == PlayerLogic.Character.Victini)
        {
            int victiniRoll = DiceRollNumber();
            if(victiniRoll < 3)
            {
                StartCoroutine(gameManger.GetCurrentPlayer().MainMovement(4));
            }
            else
            {
                StartCoroutine(gameManger.GetCurrentPlayer().MainMovement(victiniRoll));
            }
        }

        else if (gameManger.GetCurrentPlayer().character == PlayerLogic.Character.Golisopod)
        {
            int roll = DiceRollNumber();
            if(roll > 1)
            {
                StartCoroutine(gameManger.GetCurrentPlayer().MainMovement(roll * 2));
            }
            else
            {
                int rollToStart = -1 * gameManger.GetCurrentPlayer().currentTile.GetComponent<TileLogic>().id;
                StartCoroutine(gameManger.GetCurrentPlayer().MainMovement(rollToStart));
            }

        }

        else
        {
            StartCoroutine(gameManger.GetCurrentPlayer().MainMovement(DiceRollNumber()));
        }
        
   
    }

    [ObserversRpc]
    public void ShowDiceUI()
    {
        diceButton.gameObject.SetActive(true);
    }

    [ObserversRpc]
    public void HideDiceUI()
    {
        diceButton.gameObject.SetActive(false);
    }



    [ServerRpc]
    public void RollDiceServerRpc()
    {
        isDicePressed = true;
    }






    public int DiceRollNumber()
    {
        return gameManger.GetCurrentPlayer().moveNum;
    }












}
