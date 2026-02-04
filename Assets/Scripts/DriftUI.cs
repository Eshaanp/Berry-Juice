using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PurrNet;
public class DriftUI : NetworkBehaviour
{
    public GameManger gameManger;

    [Header("Main Move")]
    public Button diceButton;
    

    [Header("Driflim")]
    public Button DoubleButton;
    public Button DontDoubleButton;
    public bool driftButtonPressed = false;
    public bool doubleRoll = false;



    public IEnumerator DoubleForTrip(PlayerLogic player, int firstRoll)
    {


        if (!isServer)
        {
            yield break;
        }
        driftButtonPressed = false;
        doubleRoll = false;

        // Enable buttons when asking
        showMeowUI(true);

        // Wait for a button press
        while (!driftButtonPressed)
        {
            yield return null;
        }

        // Disable buttons immediately
        showMeowUI(false);

        if (doubleRoll)
        {
            int doubledRoll = firstRoll * 2;
            Debug.Log("double roll: " + doubledRoll);
            player.skipTurn = true; // trip player 
            player.StartMainMovement(doubledRoll);
        }
        else
        {
            Debug.Log("Keeping first roll");
            player.StartMainMovement(firstRoll);
        }
    }




    //UI Management for Meowscarada re roll choice

    public void DriftChoiceUI()
    {
        if (!isServer)
        {
            return;
        }
        driftButtonPressed = false;
        doubleRoll = false;

        // Enable buttons when asking
        //showUI();
        
    }



    /* Buttons to pick whether player double roll and trip
     * only shown to play with client id = to turn 
     * 
     */
    public void DoubleYesPressed()
    {
        DoubleToServer(true);
    }
    public void DoubleNoPressed()
    {
        DoubleToServer(false);
    }
    [ServerRpc]
    private void DoubleToServer(bool isdouble, RPCInfo info = default)
    {
        if ((ushort)info.sender.id != (ushort)gameManger.currentPlayerTurn.value)
        {
            return;
        }
        doubleRoll = isdouble;
        driftButtonPressed = true;
    }



    /* Buttons to show drift UI
     * all clients send local id to server
     * server picks which one to show based on its id
     */
    [ObserversRpc]
    public void showMeowUI(bool showUI)
    {
        PlayerID clientId = localPlayer.Value;
        ShowDriftUIServer(clientId, showUI);
    }
    [ServerRpc]
    private void ShowDriftUIServer(PlayerID target, bool showUI)
    {
        if ((ushort)gameManger.currentPlayerTurn.value == (ushort)target.id)
        {
            showDriftUITarget(target, showUI);
        }
    }
    [TargetRpc]
    public void showDriftUITarget(PlayerID target, bool showUI)
    {
        DoubleButton.gameObject.SetActive(showUI);
        DontDoubleButton.gameObject.SetActive(showUI);
    }



}
