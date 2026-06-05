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
        TopsyTurvy,
        LightThatBurn,
        Coaching,
        Present,
        BloodMoon,
        DireClaw,
        JumpKick,
        SpiritShackle,
        MakeItRain,

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
    public Sprite LightThatBurnImage;
    public Sprite CoachingImage;
    public Sprite PresentImage;
    public Sprite BloodMoonImage;
    public Sprite DireClawImage;
    public Sprite JumpKickImage;
    public Sprite SpiritShackleImage;
    public Sprite MakeItRainImage;


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

            case CardType.LightThatBurn:
                image.sprite = LightThatBurnImage;
                break;

            case CardType.Coaching:
                image.sprite = CoachingImage;
                break;

            case CardType.Present:
                image.sprite = PresentImage;
                break;

            case CardType.BloodMoon:
                image.sprite = BloodMoonImage;
                break;

            case CardType.DireClaw:
                image.sprite = DireClawImage;
                break;

            case CardType.JumpKick:
                image.sprite = JumpKickImage;
                break;

            case CardType.SpiritShackle:
                image.sprite = SpiritShackleImage;
                break;

            case CardType.MakeItRain:
                image.sprite = MakeItRainImage;
                break;


            case CardType.EmptyCard:
                image.sprite = empty;
                break;


        }

    }



}
