using PurrNet;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using static Unity.Collections.AllocatorManager;
using Unity.Networking.Transport;
using System;

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
    public int moveNum = 1;


    private void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            
            //StartCoroutine(diceRoll());
        }
    }

    public IEnumerator diceRoll() 
    {
       
        
        if (!isServer)
        {
            yield break;
        }
        

        isDicePressed = false;


        
        ShowDiceUIObserver(true);

        while (!isDicePressed)
        {
            yield return null;
        }
        isDicePressed = true;
        ShowDiceUIObserver(false);

        // Debug.Log("Dice rolled a 2");


        if (currentPlayer().character == PlayerLogic.Character.Meowscarada)
        {

            Debug.Log("Your First Roll is " + DiceRollNumber() + ". Roll again? (y/n)");
            StartCoroutine(meowManager.ReRollChoice(currentPlayer(), DiceRollNumber()));

            //characterType.ReRoll(currentPlayer(), DiceRollNumber());
        }

        else if (currentPlayer().character == PlayerLogic.Character.Drifblim)
        {
            Debug.Log("Your First Roll is " + DiceRollNumber() + ". Double for a trip? (y/n)");
            StartCoroutine(driftManager.DoubleForTrip(currentPlayer(), DiceRollNumber()));
            //characterType.ReRoll(currentPlayer(), DiceRollNumber());
        }
        else if (currentPlayer().character == PlayerLogic.Character.Victini)
        {
            int victiniRoll = DiceRollNumber();
            if(victiniRoll < 3)
            {
                StartCoroutine(currentPlayer().MainMovement(4));
            }
            else
            {
                StartCoroutine(currentPlayer().MainMovement(victiniRoll));
            }
        }

        else if (currentPlayer().character == PlayerLogic.Character.Golisopod)
        {
            int roll = DiceRollNumber();
            if(roll > 1)
            {
                StartCoroutine(currentPlayer().MainMovement(roll * 2));
            }
            else
            {
                int rollToStart = -1 * currentPlayer().currentTile.GetComponent<TileLogic>().id;
                currentPlayer().Teleport(gameManager.firstTile);
                StartCoroutine(currentPlayer().MainMovement(0));
            }

        }
        else if (currentPlayer().character == PlayerLogic.Character.Raboot)
        {
            int roll = DiceRollNumber();
            RabootEffectModifier(roll);

        }

        else
        {
            StartCoroutine(currentPlayer().MainMovement(DiceRollNumber()));
        }
        
   
    }

    [ObserversRpc]
    public void ShowDiceUIObserver(bool showUI)
    {

        GetRpcInfoDiceUI(showUI);


    }

    [ServerRpc]
    private void GetRpcInfoDiceUI(bool showUI, RPCInfo info = default)
    {
        //Here we now have the RPC info
        //Debug.Log($"Sender: {info.sender}");

        if ((ulong)gameManager.currentPlayerTurn.value == (ulong)info.sender.id)
        {
            showUITarget(info.sender, showUI);
        }

    }



    [TargetRpc]
    public void showUITarget(PlayerID target, bool showUI)
    {

        diceButton.gameObject.SetActive(showUI);
     
    }



    
    public void RollDiceServerRpc()
    {

        TestRpc();



    }


    [ServerRpc]
    private void TestRpc(RPCInfo info = default)
    {
        //Here we now have the RPC info
        Debug.Log($"Sender: {info.sender}");

        ulong id = info.sender.id;
        ulong turn = (ulong)gameManager.currentPlayerTurn.value;
        //Debug.Log(id);
        //Debug.Log(turn);
        if (id != turn)
        {
            return;
        }

        isDicePressed = true;

    }






    public int DiceRollNumber()
    {
        return gameManager.GetCurrentPlayer().moveNum;
    }


    public PlayerLogic currentPlayer(){
        
        return gameManager.GetCurrentPlayer();
    }



    public void RabootEffectModifier(int rollNum)
    {

        PlayerLogic[] players = gameManager.getAllPlayers();

        int[] placements = new int[gameManager.numOfPlayers];

        for (int i = 0; i < gameManager.numOfPlayers; i++)
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


        StartCoroutine(currentPlayer().MainMovement((current-1)-currentSpaceId));



    }







}
