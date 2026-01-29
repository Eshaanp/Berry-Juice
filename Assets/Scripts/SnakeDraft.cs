using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PurrNet;
using UnityEditor.Experimental.GraphView;

public class SnakeDraft : NetworkBehaviour
{

    //Manages snake draft logic and UI for it

    public GameManger gameManager;
    //public UIManager uiManager;
    public bool didPlayerPressButton = false;

    [Header("Snake Draft Bools")]
    public bool isMeowscarada = false;
    public bool isPatrat = false;
    public bool isJigglypuff = false;
    public bool isLuvdisc = false;
    public bool isSligoo = false;
    public bool isGolisopod = false;
    public bool isHoopa = false;
    public bool isVictini = false;
    public bool isOricorio = false;
    public bool isRaboot = false;
    public bool isDrifblim = false;

    [Header("Snake Draft Buttons")]
    public Button Meowscarada;
    public Button Jigglypuff;
    public Button Luvdisc;
    public Button Sligoo;
    public Button Patrat;
    public Button Hoopa;
    public Button Golisopod;
    public Button Victini;
    public Button Oricorio;
    public Button Raboot;
    public Button Drifblim;


    //Turn off and on all select character buttons 
    [ObserversRpc]
    public void CharacterDraftButtonsActive()
    {
        //this.gameObject.SetActive(true);
        Meowscarada.gameObject.SetActive(true);
        Jigglypuff.gameObject.SetActive(true);
        Luvdisc.gameObject.SetActive(true);
        Sligoo.gameObject.SetActive(true);
        Patrat.gameObject.SetActive(true);
        Golisopod.gameObject.SetActive(true);
        Hoopa.gameObject.SetActive(true);
        Victini.gameObject.SetActive(true);
        Oricorio.gameObject.SetActive(true);
        Raboot.gameObject.SetActive(true);
        Drifblim.gameObject.SetActive(true);

    }

    [ObserversRpc]
    public void CharacterDraftButtonsDeActive()
    {
        Meowscarada.gameObject.SetActive(false);
        Jigglypuff.gameObject.SetActive(false);
        Luvdisc.gameObject.SetActive(false);
        Sligoo.gameObject.SetActive(false);
        Patrat.gameObject.SetActive(false);
        Golisopod.gameObject.SetActive(false);
        Hoopa.gameObject.SetActive(false);
        Victini.gameObject.SetActive(false);
        Oricorio.gameObject.SetActive(false);
        Raboot.gameObject.SetActive(false);
        Drifblim.gameObject.SetActive(false);
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



        gameManager.currentPlayerTurn.value = gameManager.numOfPlayers;
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
            gameManager.currentPlayerTurn.value -= 1;
        }

        CharacterDraftButtonsDeActive();
        this.gameObject.SetActive(false);
        didPlayerPressButton = false;
        gameManager.currentPlayerTurn.value = 1;

    }

    //waiting for player to pick character
    
    private IEnumerator PickCharacter(PlayerLogic player)
    {
        

        while (!didPlayerPressButton)
        {
            yield return null;
        }


    }



    //Buttons call these functions 
    [ServerRpc]
    public void PressMeowscarada()
    {
        if (!isMeowscarada)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Meowscarada);
            Meowscarada.gameObject.SetActive(false);
            turnOff(1);
            isMeowscarada = true;
            didPlayerPressButton = true;

        }
    }



    [ServerRpc]
    public void PressLuvdisc()
    {
        if (!isLuvdisc)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Luvdisc);
            Luvdisc.gameObject.SetActive(false);
            turnOff(2);
            isLuvdisc = true;
            didPlayerPressButton = true;
        }
    }
    [ServerRpc]
    public void PressSligoo()
    {
        if (!isSligoo)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Sligoo);
            Sligoo.gameObject.SetActive(false);
            turnOff(3);
            isSligoo = true;
            didPlayerPressButton = true;
        }
    }
    [ServerRpc]
    public void PressPatrat()
    {
        if (!isPatrat)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Patrat);
            Patrat.gameObject.SetActive(false);
            turnOff(4);
            isPatrat = true;
            didPlayerPressButton = true;
        }
    }
    [ServerRpc]
    public void PressJigglypuff()
    {
        if (!isJigglypuff)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Jigglypuff);
            Jigglypuff.gameObject.SetActive(false);
            turnOff(5);
            isJigglypuff = true;
            didPlayerPressButton = true;
        }
    }
    [ServerRpc]
    public void PressGolisopod()
    {
        if (!isGolisopod)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Golisopod);
            Golisopod.gameObject.SetActive(false);
            turnOff(6);
            isGolisopod = true;
            didPlayerPressButton = true;
        }
    }
    [ServerRpc]
    public void PressVictini()
    {
        if (!isVictini)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Victini);
            Victini.gameObject.SetActive(false);
            turnOff(7);
            isVictini = true;
            didPlayerPressButton = true;
        }
    }
    [ServerRpc]
    public void PressHoopa()
    {
        if (!isHoopa)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Hoopa);
            Hoopa.gameObject.SetActive(false);
            turnOff(8);
            isHoopa = true;
            didPlayerPressButton = true;
        }
    }

    [ServerRpc]
    public void PressOricorio()
    {
        if (!isOricorio)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Oricorio);
            Oricorio.gameObject.SetActive(false);
            turnOff(9);
            isOricorio = true;
            didPlayerPressButton = true;
        }
    }

    [ServerRpc]
    public void PressRaboot()
    {
        if (!isRaboot)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Raboot);
            Raboot.gameObject.SetActive(false);
            turnOff(10);
            isRaboot = true;
            didPlayerPressButton = true;
        }
    }

    [ServerRpc]
    public void PressDrifblim()
    {
        if (!isDrifblim)
        {
            gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Drifblim);
            Drifblim.gameObject.SetActive(false);
            turnOff(11);
            isDrifblim = true;
            didPlayerPressButton = true;
        }
    }





    [ObserversRpc]
    public void turnOff(int num)
    {
        switch (num)
        {
            case 1:
                Meowscarada.gameObject.SetActive(false);
                break;
            case 2:
                Luvdisc.gameObject.SetActive(false);
                break;
            case 3:
                Sligoo.gameObject.SetActive(false);
                break;
            case 4:
                Patrat.gameObject.SetActive(false);
                break;
            case 5:
                Jigglypuff.gameObject.SetActive(false);
                break;
            case 6:
                Golisopod.gameObject.SetActive(false);
                break;
            case 7:
                Victini.gameObject.SetActive(false);
                break;
            case 8:
                Hoopa.gameObject.SetActive(false);
                break;
            case 9:
                Oricorio.gameObject.SetActive(false);
                break;
            case 10:
                Raboot.gameObject.SetActive(false);
                break;
            case 11:
                Drifblim.gameObject.SetActive(false);
                break;


        }
        
        
     
    }

}
