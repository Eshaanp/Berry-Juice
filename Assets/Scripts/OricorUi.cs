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

        ShowOriUI(true);    

        while (!oriButtonPressed)
        {
            yield return null;
        }
        ShowOriUI(false);
        StartCoroutine(roll.diceRoll());
    }



    [ObserversRpc]
    private void ShowOriUI(bool showUI)
    {
        PlayerID clientId = localPlayer.Value;
        ShowOriUIServer(clientId, showUI);

    }
    [ServerRpc]
    public void ShowOriUIServer(PlayerID target, bool showUI)
    {
        if ((ushort)gameManger.currentPlayerTurn.value == (ushort)target.id)
        {
            ShowOriUITarget(target, showUI);
        }
    }
    [TargetRpc]
    public void ShowOriUITarget(PlayerID target, bool showUI)
    {
        Cheer.gameObject.SetActive(showUI);
        noCheer.gameObject.SetActive(showUI);
    }




    public void PickNoCheer()
    {
        CheerToServer(false);
    }
    public void PickCheer()
    {
        CheerToServer(true);
    }
    [ServerRpc]
    private void CheerToServer(bool isCheer, RPCInfo info = default)
    {
        if ((ushort)info.sender.id != (ushort)gameManger.currentPlayerTurn.value)
        {
            return;
        }

        oriButtonPressed = true;

        if (isCheer)
        {
            movePlayers();
        }

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
                    
                    lastPlace[i].StartSlide(2);
                }

            }

            gameManger.GetCurrentPlayer().StartSlide(1);
        }
    }




















}
