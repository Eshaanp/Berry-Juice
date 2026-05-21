using UnityEngine;
using UnityEngine.InputSystem;
using static TileLogic;
using System.Collections;
using UnityEngine.UI;
using PurrNet;
using Unity.Burst.Intrinsics;
using System.Linq;
using System.Collections.Generic;



public class PlayerTypes : NetworkBehaviour
{

    //Manages how the player's character effects are played 

    public GameManger gameManager; 
    public MeowUI meowManager;
    public HoopaUI hoopaManager;
    public OricorUi oriManager;
    public DiceRoll roll;



    private void Awake()
    {
        //ReRollButton.onClick.AddListener(OnYesPressed);
        //DontReRollButton.onClick.AddListener(OnNoPressed);
    }


    //Checks the character before the main roll 
    //Currently Meowcarada, Luvdisc 
    public void CheckCharacterBeforeRole(PlayerLogic player)
    {
        //Debug.Log(player.character);
        switch (player.character)
        {
            case PlayerLogic.Character.Meowscarada:
                StartCoroutine(roll.diceRoll());
                break;

            case PlayerLogic.Character.Victini:
                StartCoroutine(roll.diceRoll());
                break;

            case PlayerLogic.Character.Golisopod:
                StartCoroutine(roll.diceRoll());
                break;

            case PlayerLogic.Character.Oricorio:
                StartCoroutine(oriManager.Oricorio());
                break;

            case PlayerLogic.Character.Hoopa:
                StartCoroutine(hoopaManager.Hoopa());
                break;

            case PlayerLogic.Character.Luvdisc:
                CheckIfLastPlace(player);
                StartCoroutine(roll.diceRoll());
                break;

            default:
                StartCoroutine(roll.diceRoll());
                break;
        }
  
    }

    //Checks the character during the main movement
    //Currently sligoo, jigglypuff
    public void CheckCharacterDuringRole(PlayerLogic player)
    {
        //Debug.Log(player.character);
        switch (player.character)
        {
            case PlayerLogic.Character.Sligoo:
                MoveBackwards(player);

                break;

            default:
                CheckJigglypuffEffect(player);
                break;
        }

    }






    //JiggyPuff effect- if passby, trip player. called by Check During ROle
    private void CheckJigglypuffEffect(PlayerLogic player)
    {
        if (gameManager.GetCurrentPlayer().character == PlayerLogic.Character.Jigglypuff)
        {
            if (player.PlayerId != 1 && gameManager.player1.character == PlayerLogic.Character.Jigglypuff && player.currentTile == gameManager.player1.currentTile)
            {
                player.skipTurn = true;

            }
            if (player.PlayerId != 2 && gameManager.player2.character == PlayerLogic.Character.Jigglypuff && player.currentTile == gameManager.player2.currentTile)
            {
                player.skipTurn = true;
            }
            if (gameManager.numOfPlayers >= 3 && player.PlayerId != 3 && gameManager.player3.character == PlayerLogic.Character.Jigglypuff && player.currentTile == gameManager.player3.currentTile)
            {
                player.skipTurn = true;
            }
            if (gameManager.numOfPlayers == 4 && player.PlayerId != 4 && gameManager.player4.character == PlayerLogic.Character.Jigglypuff && player.currentTile == gameManager.player4.currentTile)
            {
                player.skipTurn = true;
            }
        }
    }



    /* Currently Rerolls for meowscarada, uses MeowUI for all logic currently
     * currently, meowscarada calls the normal DiceRoll function, which directs it here after determining the first roll
     * may combine later (more self containment)
     */
    public void ReRoll(PlayerLogic player, int firstRoll)
    {

        
        Debug.Log("Your First Roll is " + firstRoll + ". Roll again? (y/n)");
        StartCoroutine(meowManager.ReRollChoice(player, firstRoll));


    }





    /*
     * Sligoo- -1 spaces to players it passes
     * 
     * 
     */
    private void MoveBackwards(PlayerLogic player)
    {
        if (gameManager.GetCurrentPlayer().character == PlayerLogic.Character.Sligoo)
        {
            if (player.PlayerId != 1 && player.currentTile == gameManager.player1.currentTile)
            {
                Debug.Log("passing player 1");
                gameManager.player1.SlideSpriteChange(true);
                gameManager.player1.StartSlide(-1);
            }
            if (player.PlayerId != 2 && player.currentTile == gameManager.player2.currentTile)
            {
                Debug.Log("passing player 2");
                gameManager.player2.SlideSpriteChange(true);
                gameManager.player2.StartSlide(-1);
            }
            if (gameManager.numOfPlayers >= 3 && player.PlayerId != 3 && player.currentTile == gameManager.player3.currentTile)
            {
                Debug.Log("passing player 3");
                gameManager.player3.SlideSpriteChange(true);
                gameManager.player3.StartSlide(-1);
            }
            if (gameManager.numOfPlayers == 4 && player.PlayerId != 4 && player.currentTile == gameManager.player4.currentTile)
            {
                Debug.Log("passing player 4");
                gameManager.player4.SlideSpriteChange(true);
                gameManager.player4.StartSlide(-1);
            }
        }

    }


    //For Luvdisc, checks if its in last place, decrease score if it is
    private void CheckIfLastPlace(PlayerLogic player)
    {
        //int player1Place = gameManager.player1.currentTile.GetComponent<TileLogic>().id;
       //int player2Place = gameManager.player2.currentTile.GetComponent<TileLogic>().id;
        //int player3Place = gameManager.player3.currentTile.GetComponent<TileLogic>().id;
        //int player4Place = gameManager.player4.currentTile.GetComponent<TileLogic>().id;

        List<PlayerLogic> lastPlacePlayers = gameManager.getLastPlacePlayers();
        for (int i = 0; i < lastPlacePlayers.Count; i++)
        {
            if (lastPlacePlayers[i] == player)
            {
                switch (player.PlayerId)
                {
                    case 1:
                        gameManager.Player1Score -= 1;
                        break;
                    case 2:
                        gameManager.Player2Score -= 1;
                        break;
                    case 3:
                        gameManager.Player3Score -= 1;
                        break;
                    case 4:
                        gameManager.Player4Score -= 1;
                        break;
                }
            }
        }



  




    }
    

}
