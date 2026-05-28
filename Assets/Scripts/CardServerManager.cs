using PurrNet;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public bool stickyWeb;
    public int stickyWebOrigin;

    public bool snatch;
    public int snatchOrgin;

    public bool topsyTurvy;
    public int topsyTurvyOrgin;

    public bool taunt;
    public int tauntOrgin;


    public void Snatch()
    {
        snatch = true;
        snatchOrgin = gameManger.GetCurrentPlayer().PlayerId;
        snatchInEffect(true);

    }

    public void StickyWeb()
    {
        stickyWeb = true;
        stickyWebOrigin = gameManger.GetCurrentPlayer().PlayerId;
        stickyInEffect(true);
    }

    public void TopsyTurvy()
    {
        topsyTurvy = true;
        topsyTurvyOrgin = gameManger.GetCurrentPlayer().PlayerId;
        topsyInEffect(true);
    }

    public void Taunt()
    {
        taunt = true;
        tauntOrgin = gameManger.GetCurrentPlayer().PlayerId;
        tauntInEffect(true);
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

    public void Teleport()
    {
        PlayerLogic[] allPlayers = gameManger.getAllPlayers();

        int len = allPlayers.Length;
        int randomInt = UnityEngine.Random.Range(0, len);

        gameManger.GetCurrentPlayer().StartTeleport(allPlayers[randomInt].currentTile);

    }

    public void Ingrain()
    {
        gameManger.GetCurrentPlayer().skipTurn = true;
        gameManger.updateScore(gameManger.GetCurrentPlayer(), 2);
    }

   
    public void HeartSwap()
    {
        PlayerLogic[] allPlayers = gameManger.getAllPlayers();
        PlayerLogic[] targetPlayers = new PlayerLogic[gameManger.numOfPlayers - 1];
        int index = 0;

        for (int i = 0; i < allPlayers.Length; i++)
        {
            if (allPlayers[i] != gameManger.GetCurrentPlayer())
            {
                targetPlayers[index] = allPlayers[i];
                index++;
            }
        }

        int randomInt = UnityEngine.Random.Range(0, gameManger.numOfPlayers - 1);

        swapCharacters(targetPlayers[randomInt]);

    }

    public void swapCharacters(PlayerLogic target)
    {
        //check infrastructure

        PlayerLogic.Character currentPlayerCharacter = gameManger.GetCurrentPlayer().character;

        PlayerLogic.Character targetPlayerCharacter = target.character;

        //gameManger.GetCurrentPlayer().character = targetPlayerCharacter;
        //target.character = currentPlayerCharacter;

        
        //turn off sprites
        gameManger.GetCurrentPlayer().TurnOffAllSprites();
        target.TurnOffAllSprites();

        //turn on new sprites
        gameManger.GetCurrentPlayer().SetUpCharacter(targetPlayerCharacter);
        target.SetUpCharacter(currentPlayerCharacter);
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
    [ObserversRpc]
    public void tauntInEffect(bool turnOn)
    {
        clientCardManager.isTauntInEffect = turnOn;
    }
    [ObserversRpc]
    public void topsyInEffect(bool turnOn)
    {
        clientCardManager.isTopsyInEffect = turnOn;
    }

    [ObserversRpc]
    public void clientCanPlayCards()
    {
        clientCardManager.playerUsedCard = false;
    }



}
