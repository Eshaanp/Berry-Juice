using System.Collections;
using System.Collections.Generic;
using System.Net;
using PurrNet;
using UnityEngine;
public class AI_Manager : NetworkBehaviour
{
   
    
    public GameManger gameManager;
    public CardServerManager cardServerManager;
    public DiceRoll diceManager;
    public HoopaUI HoopaAbility;
    public OricorUi OricorioAbility;


    public IEnumerator AI_Turn()
    {
        if (!isServer)
        {
            yield break;
        }

        yield return new WaitForSeconds(1f);

        if (gameManager.GetCurrentPlayer().character == PlayerLogic.Character.Hoopa)
        {
            
            HoopaNPC();
        }
        else if(gameManager.GetCurrentPlayer().character == PlayerLogic.Character.Oricorio)
        {
            PlusleNPC();
        }




        StartCoroutine(AICardPhase());


    }

    public IEnumerator AICardPhase()
    {
        yield return new WaitForSeconds(1f);

        ActionCard.CardType[] cardArray = new ActionCard.CardType[3];


        switch (gameManager.GetCurrentPlayer().PlayerId)
        {
            case 1: cardArray = cardServerManager.Player1Cards; break;
            case 2: cardArray = cardServerManager.Player2Cards; break;
            case 3: cardArray = cardServerManager.Player3Cards; break;
            case 4: cardArray = cardServerManager.Player4Cards; break;
        }


        for (int i = 0; i < 3; i++)
        {
            if (cardArray[i] != ActionCard.CardType.EmptyCard)
            {
                AIPlayCard(cardArray[i]);
                cardArray[i] = ActionCard.CardType.EmptyCard;
                
                break;

            }
        }



        switch (gameManager.GetCurrentPlayer().PlayerId)
        {
            case 1: cardServerManager.Player1Cards = cardArray; break;
            case 2: cardServerManager.Player2Cards = cardArray; break;
            case 3: cardServerManager.Player3Cards = cardArray; break;
            case 4: cardServerManager.Player4Cards = cardArray; break;
        }


        yield return new WaitForSeconds(1f);
        Debug.Log("Ai will roll dice");
        StartCoroutine(diceManager.diceRoll());

    }


    public void NPCDraft(bool testing)
    {


        if (!testing)
        {
            int playerNum = gameManager.numOfPlayers;
            PlayerLogic[] player = gameManager.getAllPlayers();
            List<PlayerLogic.Character> chosenCharacters = new List<PlayerLogic.Character>();

            for (int i = 0; i < playerNum; i++)
            {
                chosenCharacters.Add(player[i].pickedCharacters[0]);
                chosenCharacters.Add(player[i].pickedCharacters[1]);
            }

            List<PlayerLogic.Character> characters = new List<PlayerLogic.Character>
            {
                PlayerLogic.Character.Patrat,
                PlayerLogic.Character.Jigglypuff,
                PlayerLogic.Character.Sligoo,
                PlayerLogic.Character.Meowscarada,
                PlayerLogic.Character.Luvdisc,
                PlayerLogic.Character.Victini,
                PlayerLogic.Character.Golisopod,
                PlayerLogic.Character.Hoopa,
                PlayerLogic.Character.Oricorio,
                PlayerLogic.Character.Raboot,
                PlayerLogic.Character.Drifblim
            };
            characters.RemoveAll(c => chosenCharacters.Contains(c));



            int numOfNPC = 4 - playerNum;



            for (int i = 0; i < numOfNPC; i++)
            {
                PlayerLogic.Character randomChar1 = characters[Random.Range(0, characters.Count)];
                characters.Remove(randomChar1);
                player[3 - i].pickedCharacters.Add(randomChar1);

                PlayerLogic.Character randomChar2 = characters[Random.Range(0, characters.Count)];
                characters.Remove(randomChar2);
                player[3 - i].pickedCharacters.Add(randomChar2);
            }
        }
        else
        {
            gameManager.player3.pickedCharacters.Add(PlayerLogic.Character.Oricorio);
            gameManager.player3.pickedCharacters.Add(PlayerLogic.Character.Hoopa);


        }

    }








    public void HoopaNPC()
    {
        List<PlayerLogic> placementList = gameManager.getPlayersInPlacementOrder();
        Debug.Log(placementList);

        PlayerLogic firstPlayer = placementList[3];
        PlayerLogic secondPlayer = placementList[2];
        Debug.Log("1");

        Debug.Log("2");
        if (firstPlayer != gameManager.GetCurrentPlayer() && firstPlayer.CrossedFinish == false)
        {
            Debug.Log("3");
            int chance = Random.Range(0, 1);
            if(chance == 0 && firstPlayer.CurrentTileId - gameManager.GetCurrentPlayer().CurrentTileId > 3)
            {
                HoopaAbility.movePlayer(firstPlayer.PlayerId);
                Debug.Log("Ai Teleports first place");
            }
            else if (secondPlayer != gameManager.GetCurrentPlayer() && secondPlayer.CrossedFinish == false){

                chance = Random.Range(0, 2);
                if (chance == 0 && secondPlayer.CurrentTileId - gameManager.GetCurrentPlayer().CurrentTileId > 3)
                {
                    HoopaAbility.movePlayer(secondPlayer.PlayerId);
                    Debug.Log("Ai Teleports second place");
                }
            }
        }
        
    }


    public void PlusleNPC()
    {
        List<PlayerLogic> lastPlace = gameManager.getLastPlacePlayers();
        
        int chance = Random.Range(0, 2);

        if(chance == 0)
        {
            OricorioAbility.movePlayers(); 
            Debug.Log("Ai Cheers");
        }
        
    }
    public void AIPlayCard(ActionCard.CardType card)
    {
        Debug.Log("AI Playing: " + card);
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
                if (cardServerManager.stickyWeb == false) cardServerManager.StickyWeb();
                break;

            case ActionCard.CardType.Snatch:
                if (cardServerManager.snatch == false) cardServerManager.Snatch();
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
                if (cardServerManager.taunt == false) cardServerManager.Taunt();
                break;

            case ActionCard.CardType.TopsyTurvy:
                if (cardServerManager.topsyTurvy == false) cardServerManager.TopsyTurvy();
                break;
        }


    }




}
