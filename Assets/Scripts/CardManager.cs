using PurrNet;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static ActionCard;

public class CardManager : NetworkBehaviour
{


    public GameObject cardCanvas;
    public GameObject MainCanvas;

    private CardType[] cardArr = { CardType.Agility, CardType.GigaImpact, CardType.Hypnosis, CardType.Hypnosis, CardType.Snatch };
    private float[] probabilities = { 0.24f, 0.24f, 0.24f, 0.14f, 0.14f };

    [Header("Card Buttons")]
    public ActionCard cardButton1;
    public ActionCard cardButton2;
    public ActionCard cardButton3;

    [Header("Card Checks")]
    public CardServerManager cardServerManager;

    [Header("Cards In Effect")]
    public bool isStickyInEffect;
    public bool isSnatchInEffect;
    public bool isTopsyInEffect;
    public bool isTauntInEffect;

    public bool playerUsedCard = false;







    public CardType getRandomCard()
    {
        float roll = Random.value; // 0.0–1.0
        float cumulative = 0f;

        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulative += probabilities[i];

            if (roll <= cumulative)
                return cardArr[i];
        }
        return cardArr[cardArr.Length - 1];
    }

    public void generateCard()
    {
        Debug.Log("In generate Card");
        addCard(ActionCard.CardType.TopsyTurvy);
        //addCard(getRandomCard());
    }

    public void addCard(CardType card)
    {
        ActionCard cardSlot;
        if (cardButton1.cardType == CardType.EmptyCard)
        {
            cardSlot = cardButton1;
        }
        else if (cardButton2.cardType == CardType.EmptyCard)
        {
            cardSlot = cardButton2;
        }
        else if (cardButton3.cardType == CardType.EmptyCard)
        {
            cardSlot = cardButton3;
        }
        else
        {
            return;
        }

        cardSlot.SetCardSprite(card);


    }


    public void selectButton_1()
    {
        if(cardButton1.image == cardButton1.empty || checkIfInEffect(cardButton1))
        {
            Debug.Log("Empty");
            return;
        }
        checkCard(cardButton1.cardType);
        cardButton1.turnEmpty();
        cardCanvas.SetActive(false);
        MainCanvas.SetActive(true);
        playerUsedCard = true;
    }

    public void selectButton_2()
    {
        if (cardButton2.image == cardButton2.empty || checkIfInEffect(cardButton2))
        {
            return;
        }
        checkCard(cardButton2.cardType);
        cardButton2.turnEmpty();
        cardCanvas.SetActive(false);
        MainCanvas.SetActive(true);
        playerUsedCard = true;
    }

    public void selectButton_3()
    {
        if (cardButton3.image == cardButton3.empty || checkIfInEffect(cardButton3))
        {
            return;
        }
        checkCard(cardButton3.cardType);
        cardButton3.turnEmpty();
        cardCanvas.SetActive(false);
        MainCanvas.SetActive(true);
        playerUsedCard = true;
    }


    public bool checkIfInEffect(ActionCard cardSlot)
    {
        if (playerUsedCard == true) { return true; }
        if (isTauntInEffect == true) { return true; } //taunt main effect
        if (cardSlot.cardType == ActionCard.CardType.StickWeb && isStickyInEffect == true) { return true; }
        if (cardSlot.cardType == ActionCard.CardType.Snatch && isSnatchInEffect == true) { return true; }
        if (cardSlot.cardType == ActionCard.CardType.TopsyTurvy && isTopsyInEffect == true) { return true; }
        return false;

    }


    public void backButton()
    {
        cardCanvas.SetActive(false);
        MainCanvas.SetActive(true);
    }



    [ServerRpc]
    public void checkCard(CardType card)
    {
        cardServerManager.playerUsedCardThisTurn = true;
        switch (card)
        {
            case ActionCard.CardType.Agility:
                cardServerManager.agility = true;
                break;

            case ActionCard.CardType.GigaImpact:
                cardServerManager.gigaImpact = true; 
                break;

            case ActionCard.CardType.Hypnosis:
                cardServerManager.Hypnosis();
                break;

            case ActionCard.CardType.StickWeb:
                cardServerManager.StickyWeb();
                break;

            case ActionCard.CardType.Snatch:
                cardServerManager.Snatch();
                break;

            case ActionCard.CardType.Teleport:
                cardServerManager.Teleport();
                break;

            case ActionCard.CardType.Ingrain:
                cardServerManager.Ingrain();
                break;

            case ActionCard.CardType.HeartSwap:
                cardServerManager.HeartSwap();
                break;

            case ActionCard.CardType.Taunt:
                cardServerManager.Taunt();
                break;

            case ActionCard.CardType.TopsyTurvy:
                cardServerManager.TopsyTurvy();
                break;
        }

    }


}
