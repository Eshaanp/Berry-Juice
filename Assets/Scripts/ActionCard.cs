using UnityEngine;
using UnityEngine.UI;

public class ActionCard : MonoBehaviour
{

  
    public Image image;
    public int slotNum;


    public enum CardType
    {
        EmptyCard,
        Agility,
        GigaImpact,
        Hypnosis,
        StickWeb,
        Snatch,
        Teleport,
        Ingrain,
        HeartSwap,
        Taunt,
        TopsyTurvy

    }
    [Header("Type of card")]
    public CardType cardType;

    [Header("Card Image")]
    public Sprite agilityImage;
    public Sprite gigaImpactImage;
    public Sprite hypnosisImage;
    public Sprite stickyWebImage;
    public Sprite snatchImage;
    public Sprite teleportImage;
    public Sprite ingrainImage;
    public Sprite heartSwapImage;
    public Sprite tauntImage;
    public Sprite topsyTurvyImage;

    public Sprite empty;


    private void Awake()
    {
        cardType = CardType.EmptyCard;
    }



    public void turnEmpty()
    {
        SetCardSprite(CardType.EmptyCard);
    }

    public void SetCardSprite(CardType card)
    {
        cardType = card;
        switch (cardType)
        {
            case CardType.Agility:
                image.sprite = agilityImage;
                break;

            case CardType.GigaImpact:
                image.sprite = gigaImpactImage;
                break;

            case CardType.Hypnosis:
                image.sprite = hypnosisImage;
                break;

            case CardType.StickWeb:
                image.sprite = stickyWebImage;
                break;

            case CardType.Snatch:
                image.sprite = snatchImage;
                break;

            case CardType.Teleport:
                image.sprite = teleportImage;
                break;

            case CardType.Ingrain:
                image.sprite = ingrainImage;
                break;

            case CardType.HeartSwap:
                image.sprite = heartSwapImage;
                break;

            case CardType.Taunt:
                image.sprite = tauntImage;
                break;

            case CardType.TopsyTurvy:
                image.sprite = topsyTurvyImage;
                break;

                



            case CardType.EmptyCard:
                image.sprite = empty;
                break;


        }

    }



}
