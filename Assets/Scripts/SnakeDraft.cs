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


    [ObserversRpc]
    public void setToPatrat(PlayerLogic[] currentPlayers)
    {
        for (int i = 0; i < currentPlayers.Length; i++)
        {
            currentPlayers[i].character = PlayerLogic.Character.Patrat;
        }
    }

    //First half of snake draft
    public IEnumerator StartSnakeDraft()
    {
        this.gameObject.SetActive(true);

        CharacterDraftButtons(true);

        //set to default (patrat lol) 
        PlayerLogic[] currentPlayers = gameManager.getAllPlayers();
        //setToPatrat(currentPlayers);



        for (int i = 0; i < gameManager.numOfPlayers; i++)
        {
            Debug.Log("Player Picking: " + (i+1));
            didPlayerPressButton = false;
            yield return StartCoroutine(PickCharacter());
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
            Debug.Log("Player Picking: " + (i));
            didPlayerPressButton = false;
            yield return StartCoroutine(PickCharacter());
            gameManager.currentPlayerTurn.value -= 1;
        }

        CharacterDraftButtons(false);
        this.gameObject.SetActive(false);
        didPlayerPressButton = false;
        gameManager.currentPlayerTurn.value = 1;

    }

    //waiting for player to pick character
    private IEnumerator PickCharacter()
    {
        

        while (!didPlayerPressButton)
        {
            yield return null;
        }


    }

    public bool checkIfLocalPlayerCanClick()
    {

        PlayerID clientId = localPlayer.Value;
        Debug.Log("client: " +  clientId);
        Debug.Log("turn: " + gameManager.currentPlayerTurn.value);
        if ((ushort)gameManager.currentPlayerTurn.value == (ushort) clientId.id)
        {
            return true;
        }
        return false;

    }






    [ServerRpc]
    public void draftButtonOptions(int characterNum, RPCInfo info = default)
    {
        if ((ushort)info.sender.id != (ushort)gameManager.currentPlayerTurn.value)
        {
            return;
        }

        switch (characterNum)
        {
            case 1: 
                if (!isMeowscarada)
                {
                    gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Meowscarada);
                    turnOff(1);
                    isMeowscarada = true;
                    didPlayerPressButton = true;
                }
                break;

            case 2: 
                if (!isLuvdisc)
                {
                    gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Luvdisc);
                    turnOff(2);
                    isLuvdisc = true;
                    didPlayerPressButton = true;
                }
                break;

            case 3: 
                if (!isSligoo)
                {
                    gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Sligoo);
                    turnOff(3);
                    isSligoo = true;
                    didPlayerPressButton = true;
                }
                break;

            case 4:
                if (!isPatrat)
                {
                    gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Patrat);
                    turnOff(4);
                    isPatrat = true;
                    didPlayerPressButton = true;
                }
                break;

            case 5: 
                if (!isJigglypuff)
                {
                    gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Jigglypuff);
                    turnOff(5);
                    isJigglypuff = true;
                    didPlayerPressButton = true;
                }
                break;

            case 6:
                if (!isGolisopod)
                {
                    gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Golisopod);
                    turnOff(6);
                    isGolisopod = true;
                    didPlayerPressButton = true;
                }
                break;

            case 7: 
                if (!isVictini)
                {
                    gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Victini);
                    turnOff(7);
                    isVictini = true;
                    didPlayerPressButton = true;
                }
                break;

            case 8: 
                if (!isHoopa)
                {
                    gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Hoopa);
                    turnOff(8);
                    isHoopa = true;
                    didPlayerPressButton = true;
                }
                break;

            case 9: 
                if (!isOricorio)
                {
                    gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Oricorio);
                    turnOff(9);
                    isOricorio = true;
                    didPlayerPressButton = true;
                }
                break;

            case 10: 
                if (!isRaboot)
                {
                    gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Raboot);
                    turnOff(10);
                    isRaboot = true;
                    didPlayerPressButton = true;
                }
                break;

            case 11: 
                if (!isDrifblim)
                {
                    gameManager.GetCurrentPlayer().pickedCharacters.Add(PlayerLogic.Character.Drifblim);
                    turnOff(11);
                    isDrifblim = true;
                    didPlayerPressButton = true;
                }
                break;
        }






    }


    public void PressMeowscarada()
    {
        draftButtonOptions(1);
    }

    public void PressLuvdisc()
    {
        draftButtonOptions(2);
    }

    public void PressSligoo()
    {
        draftButtonOptions(3);
    }

    public void PressPatrat()
    {
        draftButtonOptions(4);
    }

    public void PressJigglypuff()
    {
        draftButtonOptions(5);
    }

    public void PressGolisopod()
    {
        draftButtonOptions(6);
    }

    public void PressVictini()
    {
        draftButtonOptions(7);
    }

    public void PressHoopa()
    {
        draftButtonOptions(8);
    }

    public void PressOricorio()
    {
        draftButtonOptions(9);
    }

    public void PressRaboot()
    {
        draftButtonOptions(10);
    }

    public void PressDrifblim()
    {
        draftButtonOptions(11);
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

    //Turn off and on all select character buttons 
    [ObserversRpc]
    public void CharacterDraftButtons(bool showUI)
    {
        //this.gameObject.SetActive(true);
        Meowscarada.gameObject.SetActive(showUI);
        Jigglypuff.gameObject.SetActive(showUI);
        Luvdisc.gameObject.SetActive(showUI);
        Sligoo.gameObject.SetActive(showUI);
        Patrat.gameObject.SetActive(showUI);
        Golisopod.gameObject.SetActive(showUI);
        Hoopa.gameObject.SetActive(showUI);
        Victini.gameObject.SetActive(showUI);
        Oricorio.gameObject.SetActive(showUI);
        Raboot.gameObject.SetActive(showUI);
        Drifblim.gameObject.SetActive(showUI);

    }

}
