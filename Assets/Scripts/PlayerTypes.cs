using UnityEngine;
using UnityEngine.InputSystem;
using static TileLogic;
using System.Collections;

public class PlayerTypes : MonoBehaviour
{

    //find areas where a character power needs to be called and make its own function for each area its called. Pass the character type in each to check if something happens there
    /*
     * Before Main Move (Done)
     * After Main Move
     * After Tile Effect
     * During the Movement (Passing players)
     * 
     * 
     * 
     */



    public GameManger gameManger;



    public void CheckCharacterBeforeRole(PlayerLogic player)
    {
        //Debug.Log(player.character);
        switch (player.character)
        {
            case PlayerLogic.Character.Meowscarada:
                ReRoll(player);

                break;

            default:
                StartCoroutine(player.DiceRoll());
                break;
        }
  
    }


    public void CheckCharacterDuringRole(PlayerLogic player)
    {
        //Debug.Log(player.character);
        switch (player.character)
        {
            case PlayerLogic.Character.Sligoo:
                MoveBackwards(player);

                break;

            default:
                break;
        }

    }




    /* Currently Rerolls for meowscarada
     * idk why its two mehtods
     * may combine later
     */
    public void ReRoll(PlayerLogic player)
    {
        int firstRoll = player.DiceRollNumber();
        Debug.Log("Your First Roll is " + firstRoll + ". Roll again? (y/n)");
        StartCoroutine(ReRollChoice(player, firstRoll));


    }

    public IEnumerator ReRollChoice(PlayerLogic player, int firstRoll)
    {
        while (true)
        {
   
            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                int secondRoll = player.DiceRollNumber();
                Debug.Log("Your Second Roll is " + secondRoll);
                StartCoroutine(player.MainMovement(secondRoll));
                yield break; 
            }

            if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                Debug.Log("n");
                StartCoroutine(player.MainMovement(firstRoll));
                yield break; 
            }

            yield return null;
        }
    }



    /*
     * Sligoo- -2 spaces to players it passes
     * 
     * 
     */
    public void MoveBackwards(PlayerLogic player)
    {
        if(player.PlayerId != 1 && player.currentTile == gameManger.player1.currentTile)
        {
            Debug.Log("passing player 1");
            StartCoroutine(gameManger.player1.MovementSlide(-1));
        }
        if (player.PlayerId != 2 && player.currentTile == gameManger.player2.currentTile)
        {
            Debug.Log("passing player 2");
            StartCoroutine(gameManger.player2.MovementSlide(-1));
        }
        if (player.currentTile.GetComponent<TileLogic>().isPlayer3OnTile == true)
        {

        }
        if (player.currentTile.GetComponent<TileLogic>().isPlayer4OnTile == true)
        {

        }

    }



}
