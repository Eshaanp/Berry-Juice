using UnityEngine;
using UnityEngine.InputSystem;
using static TileLogic;
using System.Collections;
using UnityEngine.UI;



public class PlayerTypes : MonoBehaviour
{

    //Manages how the player's character effects are played 

    public GameManger gameManager;
    public UIManager uIManager;



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
                ReRoll(player);
                break;

            case PlayerLogic.Character.Luvdisc:
                CheckIfLastPlace(player);
                StartCoroutine(player.DiceRoll());
                break;

            default:
                StartCoroutine(player.DiceRoll());
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
        if (player.PlayerId != 1 && gameManager.player1.character == PlayerLogic.Character.Jigglypuff && player.currentTile == gameManager.player1.currentTile)
        {
            player.skipTurn = true;
            
        }
        if (player.PlayerId != 2 && gameManager.player2.character == PlayerLogic.Character.Jigglypuff && player.currentTile == gameManager.player2.currentTile)
        {
            player.skipTurn = true;
        }

    }



    /* Currently Rerolls for meowscarada, uses UIManager
     * idk why its two mehtods
     * may combine later
     */
    public void ReRoll(PlayerLogic player)
    {
        int firstRoll = player.DiceRollNumber();
        Debug.Log("Your First Roll is " + firstRoll + ". Roll again? (y/n)");
        StartCoroutine(ReRollChoice(player, firstRoll));


    }

    private IEnumerator ReRollChoice(PlayerLogic player, int firstRoll)
    {


        uIManager.MeowscaradaChoiceUI();

        // Wait for a button press
        while (!uIManager.MeowbuttonPressed)
        {
            yield return null;
        }

        // Disable buttons immediately
        uIManager.ReRollButton.gameObject.SetActive(false);
        uIManager.DontReRollButton.gameObject.SetActive(false);

        if (uIManager.reroll)
        {
            int secondRoll = player.DiceRollNumber();
            Debug.Log("Second roll: " + secondRoll);
            yield return StartCoroutine(player.MainMovement(secondRoll));
        }
        else
        {
            Debug.Log("Keeping first roll");
            yield return StartCoroutine(player.MainMovement(firstRoll));
        }
    }



    /*
     * Sligoo- -2 spaces to players it passes
     * 
     * 
     */
    private void MoveBackwards(PlayerLogic player)
    {
        if(player.PlayerId != 1 && player.currentTile == gameManager.player1.currentTile)
        {
            Debug.Log("passing player 1");
            StartCoroutine(gameManager.player1.MovementSlide(-1));
        }
        if (player.PlayerId != 2 && player.currentTile == gameManager.player2.currentTile)
        {
            Debug.Log("passing player 2");
            StartCoroutine(gameManager.player2.MovementSlide(-1));
        }
        if (player.currentTile.GetComponent<TileLogic>().isPlayer3OnTile == true)
        {

        }
        if (player.currentTile.GetComponent<TileLogic>().isPlayer4OnTile == true)
        {

        }

    }


    //For Luvdisc, checks if its in last place, decrease score if it is
    private void CheckIfLastPlace(PlayerLogic player)
    {
        int player1Place = gameManager.player1.currentTile.GetComponent<TileLogic>().id;
        int player2Place = gameManager.player2.currentTile.GetComponent<TileLogic>().id;

        if(player.currentTile.GetComponent<TileLogic>().id <= player1Place && player.currentTile.GetComponent<TileLogic>().id <= player2Place)
        {
            Debug.Log("In last place");
            switch (player.PlayerId)
            {
                case 1:
                    gameManager.Player1Score -= 1;
                    break;
                case 2:
                    gameManager.Player2Score -= 1;
                    break;
            }
        }




    }
    

}
