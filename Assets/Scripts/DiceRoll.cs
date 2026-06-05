using PurrNet;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using static Unity.Collections.AllocatorManager;
using Unity.Networking.Transport;
using System;
using System.Threading.Tasks;
using UnityEditor.PackageManager;

public class DiceRoll : NetworkBehaviour
{

    public GameManger gameManager;
    public PlayerTypes characterType;

    [Header("Specific Character Scripts")]
    public MeowUI meowManager;
    public DriftUI driftManager;


    [SerializeField] private NetworkManager networkMan;
    

    [Header("Normal Dice Stuff")]
    public bool isDicePressed = false;
    public Button diceButton;
    public Button cardButton;
    public int moveNum = 1;

    [Header("Canvas")]
    public GameObject cardCanvas;
    public GameObject MainCanvas;



    public IEnumerator diceRoll() 
    {
        if (!isServer)
        {
            yield break;
        }

        if (gameManager.GetCurrentPlayer().isAI == false)
        {
            isDicePressed = false;
            ShowDiceUIObserver(true);
            while (!isDicePressed)
            {
                yield return null;
            }
            isDicePressed = true;

            ShowDiceUIObserver(false);
        }
       
        int roll = DiceRollNumber();

        if(getCardManager().agility == true)
        {
            roll += 2;
            getCardManager().agility = false;
        } 
        else if (getCardManager().gigaImpact == true)
        {
            //turning off giga impact and side effect done in GameManager
            roll += 6;
        }

        if (getCardManager().stickyWeb == true)
        {
            //turning off at end of round
            roll -= 1;
        }
        if (getCardManager().bloodMoon == true)
        {
            if(roll > 3)
            {
                roll = 0;
            } 
           
        }
        if (getCardManager().topsyTurvy == true)
        {
            roll *= -1;
        }


        if(getCardManager().snatch && roll == 6 && currentPlayer().PlayerId != getCardManager().snatchOrgin)
        {
            roll = 0;
            gameManager.GetTargetPlayer(getCardManager().snatchOrgin).StartSlide(1);
            getCardManager().snatch = false;
            getCardManager().snatchInEffect(false);
        }

        //animation pause here

        switch (currentPlayer().character)
        {
            case PlayerLogic.Character.Meowscarada:
                Debug.Log("Your First Roll is " + roll + ". Roll again? (y/n)");
                if (!currentPlayer().isAI)
                {
                    StartCoroutine(meowManager.ReRollChoice(currentPlayer(), roll));
                }
                else
                {
                    currentPlayer().StartMainMovement(roll);
                }
                break;

            case PlayerLogic.Character.Drifblim:
                Debug.Log("Your First Roll is " + roll + ". Double for a trip? (y/n)");
                if (!currentPlayer().isAI)
                {
                    StartCoroutine(driftManager.DoubleForTrip(currentPlayer(), roll));
                }
                else
                {
                    currentPlayer().StartMainMovement(roll);
                }
                break;

            case PlayerLogic.Character.Victini:
                VictiniRoll(roll);
                break;

            case PlayerLogic.Character.Golisopod:
                GolisopodRoll(roll);
                break;

            case PlayerLogic.Character.Raboot:
                RabootEffectModifier(roll);
                break;

            default:
                
                currentPlayer().StartMainMovement(roll);
                break;

        }
        
   
    }

    /* Controls which clients get to see the dice roll button
     * all clients send local id to server
     * server picks which one to show based on its id
     * 
     */
    [ObserversRpc]
    public void ShowDiceUIObserver(bool showUI)
    {
        PlayerID clientId = localPlayer.Value;
        ShowDiceUIServer(clientId, showUI);

    }
    [ServerRpc]
    private void ShowDiceUIServer(PlayerID target, bool showUI)
    {

        if ( (ushort) gameManager.currentPlayerTurn.value == (ushort) target.id )
        {
            showUITarget(target, showUI);
        }

    }
    [TargetRpc]
    public void showUITarget(PlayerID target, bool showUI)
    {
        diceButton.gameObject.SetActive(showUI);
        cardButton.gameObject.SetActive(showUI);
    }
    
   

    
    public void RollDiceServerRpc()
    {
        RollToServer();
    }

    [ServerRpc]
    private void RollToServer(RPCInfo info = default)
    {
        if ((ushort)info.sender.id != (ushort)gameManager.currentPlayerTurn.value)
        {
            return;
        }

        isDicePressed = true;
    }

    public void CardCanvas()
    {
        MainCanvas.SetActive(false);
        cardCanvas.SetActive(true);
    }



    public int DiceRollNumber()
    {
        return gameManager.GetCurrentPlayer().moveNum;
    }


    public PlayerLogic currentPlayer(){
        
        return gameManager.GetCurrentPlayer();
    }

    public CardServerManager getCardManager()
    {
        return gameManager.cardServerManager;
    }



    public void VictiniRoll(int roll)
    {
        if (roll < 3)
        {
            currentPlayer().StartMainMovement(4);
        }
        else
        {
            currentPlayer().StartMainMovement(roll);
        }
    }

    public void GolisopodRoll(int roll)
    {
        if (roll == 1)
        {
            //int rollToStart = -1 * currentPlayer().currentTile.GetComponent<TileLogic>().id;
            currentPlayer().StartTeleport(gameManager.firstTile);
            currentPlayer().StartMainMovement(0);

        }
        else
        {
            currentPlayer().StartMainMovement(roll * 2);
        }
    }


    public void RabootEffectModifier(int rollNum)
    {

        PlayerLogic[] players = gameManager.getAllPlayers();

        int[] placements = new int[gameManager.maxPlayers];

        for (int i = 0; i < gameManager.maxPlayers; i++)
        {
            placements[i] = players[i].currentTile.gameObject.GetComponent<TileLogic>().id;
        }

        int[] blocked = placements.Distinct().ToArray();

        int currentSpaceId = currentPlayer().currentTile.gameObject.GetComponent<TileLogic>().id;



        int[] result = new int[rollNum + 1];
        int index = 0;
        int current = currentSpaceId;

        while (index <= rollNum)
        {
            bool isBlocked = false;

            for (int i = 0; i < blocked.Length; i++)
            {
                if (blocked[i] == current && blocked[i] != currentSpaceId)
                {
                    isBlocked = true;
                    break;
                }
            }

            if (!isBlocked)
            {
                result[index] = current;
                index++;
            }

            current++;
        }


        currentPlayer().StartMainMovement((current-1)-currentSpaceId);



    }







}
