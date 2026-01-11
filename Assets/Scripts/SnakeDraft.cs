using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SnakeDraft : MonoBehaviour
{

    //Manages snake draft logic and UI for it

    public GameManger gameManager;
    public UIManager uiManager;
    public bool didPlayerPressButton = false;

    [Header("Snake Draft Bools")]
    public bool isMeowscarada = false;
    public bool isPatrat = false;
    public bool isJigglypuff = false;
    public bool isLuvdisc = false;
    public bool isSligoo = false;

    [Header("Snake Draft Buttons")]
    public Button Meowscarada;
    public Button Jigglypuff;
    public Button Luvdisc;
    public Button Sligoo;
    public Button Patrat;

    
    //Turn off and on all select character buttons 
    public void CharacterDraftButtonsActive()
    {
        Meowscarada.gameObject.SetActive(true);
        Jigglypuff.gameObject.SetActive(true);
        Luvdisc.gameObject.SetActive(true);
        Sligoo.gameObject.SetActive(true);
        Patrat.gameObject.SetActive(true);

    }
    public void CharacterDraftButtonsDeActive()
    {
        Meowscarada.gameObject.SetActive(false);
        Jigglypuff.gameObject.SetActive(false);
        Luvdisc.gameObject.SetActive(false);
        Sligoo.gameObject.SetActive(false);
        Patrat.gameObject.SetActive(false);

    }


    //First half of snake draft
    public IEnumerator StartSnakeDraft()
    {
        this.gameObject.SetActive(true);
        CharacterDraftButtonsActive();

        PlayerLogic[] currentPlayers = gameManager.getAllPlayers();
        //int currentTurn = gameManager.currentPlayerTurn;

        //set to default (patrat lol) 
        for (int i = 0; i < currentPlayers.Length; i++)
        {
            currentPlayers[i].character = PlayerLogic.Character.Patrat;
        }

        for (int i = 0; i < gameManager.numOfPlayers; i++)
        {
            didPlayerPressButton = false;
            Debug.Log("Player Picking- " + (i+1));

            yield return StartCoroutine(PickCharacter(currentPlayers[i]));

            foreach (var x in gameManager.GetCurrentPlayer().pickedCharacters)
            {
                Debug.Log(x.ToString());
            }
            gameManager.NextTurn();

        }

        Debug.Log("First Draft Done");
        
        
    }

    //Snake draft in reverse order
    public IEnumerator ReverseSnakeDraft()
    {


        gameManager.currentPlayerTurn = gameManager.numOfPlayers;
        Debug.Log("Reverse. Turn: " + gameManager.currentPlayerTurn);

        for (int i = gameManager.numOfPlayers; i > 0; i--)
        {
            if (gameManager.currentPlayerTurn == 0)
            {
                break;
            }
            didPlayerPressButton = false;
            Debug.Log("Player Picking- " + (gameManager.GetCurrentPlayer().PlayerId));
            yield return StartCoroutine(PickCharacter(gameManager.GetCurrentPlayer()));
            gameManager.currentPlayerTurn -= 1;
        }

        CharacterDraftButtonsDeActive();
        this.gameObject.SetActive(false);
        didPlayerPressButton = false;
        gameManager.currentPlayerTurn = 1;

    }

    //waiting for player to pick character
    public IEnumerator PickCharacter(PlayerLogic player)
    {
        

        while (!didPlayerPressButton)
        {
            yield return null;
        }


    }



    //Buttons call these functions 
    public void PressMeowscarada()
    {
        if (!isMeowscarada)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Meowscarada);
            Meowscarada.gameObject.SetActive(false);
            isMeowscarada = true;
            didPlayerPressButton = true;

        }
    }
    public void PressLuvdisc()
    {
        if (!isLuvdisc)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Luvdisc);
            Luvdisc.gameObject.SetActive(false);
            isLuvdisc = true;
            didPlayerPressButton = true;
        }
    }
    public void PressSligoo()
    {
        if (!isSligoo)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Sligoo);
            Sligoo.gameObject.SetActive(false);
            isSligoo = true;
            didPlayerPressButton = true;
        }
    }
    public void PressPatrat()
    {
        if (!isPatrat)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Patrat);
            Patrat.gameObject.SetActive(false);
            isPatrat = true;
            didPlayerPressButton = true;
        }
    }
    public void PressJigglypuff()
    {
        if (!isJigglypuff)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Jigglypuff);
            Jigglypuff.gameObject.SetActive(false);
            isJigglypuff = true;
            didPlayerPressButton = true;
        }
    }



}
