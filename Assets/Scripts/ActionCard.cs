using UnityEngine;
using UnityEngine.UI;

public class ActionCard : MonoBehaviour
{

  
    public Image image;
    public int slotNum;


    public enum CardType
    {
        Agility,
        GigaImpact,
        Hypnosis,
        StickWeb,
        Snatch,
        EmptyCard
    }
    [Header("Type of card")]
    public CardType cardType;

    [Header("Card Image")]
    public Sprite agilityImage;
    public Sprite gigaImpactImage;
    public Sprite hypnosisImage;
    public Sprite stickyWebImage;
    public Sprite snatchImage;
    public Sprite empty;






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
                
            case CardType.EmptyCard:
                image.sprite = empty;
                break;


        }

    }



}
