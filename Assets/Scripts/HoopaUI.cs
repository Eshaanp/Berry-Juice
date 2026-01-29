using PurrNet;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoopaUI : NetworkBehaviour
{
    

    public GameManger gameManger;
    public DiceRoll roll;

    public Button player1;
    public Button player2;
    public Button player3;
    public Button player4;
    public Button noPlayer;

    public bool hoopaButtonPressed = false;




    public IEnumerator Hoopa()
    {
        hoopaButtonPressed = false;
        int currentPlayerId = gameManger.GetCurrentPlayer().PlayerId;
        int numOfPlayers = gameManger.numOfPlayers;

        ShowHoopaUI(true);

        while (!hoopaButtonPressed)
        {
            yield return null;
        }
        ShowHoopaUI(false);
        StartCoroutine(roll.diceRoll());
    }


    [ObserversRpc]
    private void ShowHoopaUI(bool showUI)
    {
        PlayerID clientId = localPlayer.Value;
        ShowHoopaUIServer(clientId, showUI);

    }
    [ServerRpc]
    public void ShowHoopaUIServer(PlayerID target, bool showUI)
    {
        if ((ushort)gameManger.currentPlayerTurn.value == (ushort)target.id)
        {
            int currentPlayerId = gameManger.GetCurrentPlayer().PlayerId;
            ShowHoopaUITarget(target, showUI, currentPlayerId);
        }
    }
    [TargetRpc]
    public void ShowHoopaUITarget(PlayerID target, bool showUI, int currentPlayerId)
    {
        if (showUI)
        {
            noPlayer.gameObject.SetActive(true);
            if (currentPlayerId != 1 && gameManger.player1.CrossedFinish == false)
            {
                player1.gameObject.SetActive(true);
            }
            if (currentPlayerId != 2 && gameManger.player2.CrossedFinish == false)
            {
                player2.gameObject.SetActive(true);
            }
            if (currentPlayerId != 3 && gameManger.numOfPlayers >= 3 && gameManger.player3.CrossedFinish == false)
            {
                player3.gameObject.SetActive(true);
            }
            if (currentPlayerId != 4 && gameManger.numOfPlayers == 4 && gameManger.player4.CrossedFinish == false)
            {
                player4.gameObject.SetActive(true);
            }

        }
        else
        {
            player1.gameObject.SetActive(false);
            player2.gameObject.SetActive(false);
            player3.gameObject.SetActive(false);
            player4.gameObject.SetActive(false);
            noPlayer.gameObject.SetActive(false);
        }
    }






    public void PickNoPlayer()
    {
        PickPlayerToServer(0);
    }
    public void PickPlayer1()
    {
        PickPlayerToServer(1);
    }
    public void PickPlayer2()
    {
        PickPlayerToServer(2);
    }
    public void PickPlayer3()
    {
        PickPlayerToServer(3);
    }
    public void PickPlayer4()
    {
        PickPlayerToServer(4);
    }

    [ServerRpc]
    private void PickPlayerToServer(int playerNum, RPCInfo info = default)
    {
        if ((ushort)info.sender.id != (ushort)gameManger.currentPlayerTurn.value)
        {
            return;
        }

        hoopaButtonPressed = true;

        if (playerNum != 0)
        {
            movePlayer(playerNum);
        }

    }






    private void movePlayer(int playerNum)
    {
        /*
        if(gameManger.GetTargetPlayer(playerNum) == null){
            return;
        }*/
        
        int currentPlayerId = gameManger.GetCurrentPlayer().PlayerId;


        int currentPlayerTileId = gameManger.GetCurrentPlayer().currentTile.GetComponent<TileLogic>().id;
        int targetPlayerTileId = gameManger.GetTargetPlayer(playerNum).currentTile.GetComponent<TileLogic>().id;

        int targetMovement = -1 * (targetPlayerTileId - currentPlayerTileId);

        if (gameManger.GetTargetPlayer(playerNum).CrossedFinish == false)
        {
            gameManger.GetTargetPlayer(playerNum).Teleport(gameManger.GetCurrentPlayer().currentTile);
        }
            


        //StartCoroutine(gameManger.GetTargetPlayer(playerNum).MovementSlide(targetMovement));

    }


    

}
