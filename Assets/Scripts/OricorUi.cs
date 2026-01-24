using PurrNet;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class OricorUi : NetworkBehaviour
{



    public GameManger gameManger;
    public DiceRoll roll;

    public Button Cheer;
    public Button noCheer;


    public bool oriButtonPressed = false;




    public IEnumerator Oricorio()
    {
        oriButtonPressed = false;
        
        setButtons();

        while (!oriButtonPressed)
        {
            yield return null;
        }
        hideButtons();
        StartCoroutine(roll.diceRoll());
    }

    [ObserversRpc]
    private void setButtons()
    {
        
        Cheer.gameObject.SetActive(true);
        noCheer.gameObject.SetActive(true);



    }

    [ObserversRpc]
    private void hideButtons()
    {
        Cheer.gameObject.SetActive(false);
        noCheer.gameObject.SetActive(false);

    }


    [ServerRpc]
    public void PickNoCheer()
    {
        oriButtonPressed = true;


    }

    [ServerRpc]
    public void PickCheer()
    {
        oriButtonPressed = true;
        movePlayers();

    }









    private void movePlayers()
    {
        List<PlayerLogic> lastPlace = gameManger.getLastPlacePlayers();
        

        if (lastPlace != null) {
            int currentPlayerId = gameManger.GetCurrentPlayer().PlayerId;

            for (int i = 0; i < lastPlace.Count; i++)
            {
                if(lastPlace[i].PlayerId != currentPlayerId)
                {
                    Debug.Log(lastPlace[i].PlayerId);
                    
                    StartCoroutine(lastPlace[i].MovementSlide(2));
                }

            }

            StartCoroutine(gameManger.GetCurrentPlayer().MovementSlide(1));
        }
    }




















}
