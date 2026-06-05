using PurrNet;
using UnityEngine;
using UnityEngine.InputSystem;
using static ActionCard;

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

    public bool bloodMoon;
    public int bloodMoonOrgin;

    [Header("Players Current Cards")]
    public ActionCard.CardType[] Player1Cards = { ActionCard.CardType.EmptyCard, ActionCard.CardType.EmptyCard, ActionCard.CardType.EmptyCard };
    public ActionCard.CardType[] Player2Cards = { ActionCard.CardType.EmptyCard, ActionCard.CardType.EmptyCard, ActionCard.CardType.EmptyCard };
    public ActionCard.CardType[] Player3Cards = { ActionCard.CardType.EmptyCard, ActionCard.CardType.EmptyCard, ActionCard.CardType.EmptyCard };
    public ActionCard.CardType[] Player4Cards = { ActionCard.CardType.EmptyCard, ActionCard.CardType.EmptyCard, ActionCard.CardType.EmptyCard };


    private CardType[] cardProb = { CardType.Agility, CardType.GigaImpact, CardType.Hypnosis, CardType.Hypnosis, CardType.Snatch };
    private float[] probabilities = { 0.24f, 0.24f, 0.24f, 0.14f, 0.14f };

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

    public void BloodMoon()
    {
        bloodMoon = true;
        bloodMoonOrgin = gameManger.GetCurrentPlayer().PlayerId;
        bloodMoonInEffect(true);
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
        PlayerLogic[] targetPlayers = new PlayerLogic[gameManger.maxPlayers - 1];
        int index = 0;

        for (int i = 0; i < allPlayers.Length; i++)
        {
            if (allPlayers[i] != gameManger.GetCurrentPlayer())
            {
                targetPlayers[index] = allPlayers[i];
                index++;
            }
        }

        int randomInt = UnityEngine.Random.Range(0, gameManger.maxPlayers - 1);

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




    public CardType getRandomCard()
    {
        float roll = Random.value; // 0.0–1.0
        float cumulative = 0f;

        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulative += probabilities[i];

            if (roll <= cumulative)
                return cardProb[i];
        }
        return cardProb[cardProb.Length - 1];
    }





    public void giveCardTargetPlayer(int playerID)
    {


        ActionCard.CardType card = getRandomCard();

        ActionCard.CardType[] cardArray = new ActionCard.CardType[3];

        Debug.Log(card.ToString());
        
        switch (playerID)
        {
            case 1: cardArray = Player1Cards; break;
            case 2: cardArray = Player2Cards; break;
            case 3: cardArray = Player3Cards; break;
            case 4: cardArray = Player4Cards; break;
        }

        for (int i = 0; i < 3; i++)
        {
            if (cardArray[i] == ActionCard.CardType.EmptyCard)
            {
                cardArray[i] = card;
                break;
            }
        }

        switch (playerID)
        {
            case 1: Player1Cards = cardArray; clientCardManager.UpdateCardToPlayer(Player1Cards, playerID); break;
            case 2: Player2Cards = cardArray; clientCardManager.UpdateCardToPlayer(Player2Cards, playerID); break;
            case 3: Player3Cards = cardArray; clientCardManager.UpdateCardToPlayer(Player3Cards, playerID); break;
            case 4: Player4Cards = cardArray; clientCardManager.UpdateCardToPlayer(Player4Cards, playerID); break;
        }


    }



    public void DeleteCard(int cardSlot)
    {

        switch (gameManger.GetCurrentPlayer().PlayerId)
        {
            case 1: Player1Cards[cardSlot] = ActionCard.CardType.EmptyCard; break;
            case 2: Player2Cards[cardSlot] = ActionCard.CardType.EmptyCard; break;
            case 3: Player3Cards[cardSlot] = ActionCard.CardType.EmptyCard; break;
            case 4: Player4Cards[cardSlot] = ActionCard.CardType.EmptyCard; break;
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
    public void bloodMoonInEffect(bool turnOn)
    {
        clientCardManager.isBloodMoonInEffect = turnOn;
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
