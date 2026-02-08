using PurrNet;
using UnityEngine;

public class CardServerManager : NetworkBehaviour
{




    public GameManger gameManger;
    public CardManager clientCardManager;

    public bool playerUsedCardThisTurn = false;

    [Header("Instant Cards")]
    public bool agility;

    public bool gigaImpact;

    //public bool hypnosis;


    [Header("Cards that can only exist one cast at a time")]
    public bool firstStickyWebCheck; 
    public bool stickyWebCheckPerPlayer;

    public bool snatch;
    public int snatchOrgin;




    public void Snatch()
    {
        snatch = true;
        snatchOrgin = gameManger.GetCurrentPlayer().PlayerId;
        snatchInEffect(true);

    }

    public void StickyWeb()
    {
        firstStickyWebCheck = true;
        stickyInEffect(true);
    }

    //Instant
    public void Hypnosis()
    {
        TileLogic currTile = gameManger.GetCurrentPlayer().currentTile.GetComponent<TileLogic>();
        int orginPlayerId = gameManger.GetCurrentPlayer().PlayerId;

        if (currTile.isPlayer1OnTile == true && orginPlayerId != 1)
        {
            gameManger.player1.skipTurn = true;
        }
        if (currTile.isPlayer2OnTile == true && orginPlayerId != 2)
        {
            gameManger.player2.skipTurn = true;
        }
        if (currTile.isPlayer3OnTile == true && orginPlayerId != 3)
        {
            gameManger.player3.skipTurn = true;
        }
        if (currTile.isPlayer4OnTile == true && orginPlayerId != 4)
        {
            gameManger.player4.skipTurn = true;
        }

    }

    [ObserversRpc]
    public void giveCardAllPlayers()
    {
        clientCardManager.generateCard();
    }

    [ObserversRpc]
    public void giveCardTargetPlayer(int playerID)
    {
        Debug.Log("Giving Card to Player: " +  playerID);
        PlayerID clientId = localPlayer.Value;

        if ((ushort)clientId.id == (ushort)playerID)
        {
            clientCardManager.generateCard();
        }

    }


    [ObserversRpc]
    public void snatchInEffect(bool turnOn)
    {
        clientCardManager.isSnatchInEffect = turnOn;
    }
    [ObserversRpc]
    public void stickyInEffect(bool turnOn)
    {
        clientCardManager.isStickyInEffect = turnOn;
    }

}
