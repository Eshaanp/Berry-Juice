using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PurrNet;

public class MeowUI : NetworkBehaviour
{

    public GameManger gameManger;

    [Header("Main Move")]
    public Button diceButton;
    

    [Header("Meowscarada")]
    public Button ReRollButton;
    public Button DontReRollButton;
    public bool MeowbuttonPressed = false;
    public bool reroll = false;


    public IEnumerator ReRollChoice(PlayerLogic player, int firstRoll)
    {

        if (!isServer)
        {
            yield break;
        }
        MeowbuttonPressed = false;
        reroll = false;

        // Enable buttons when asking
        showMeowUI(true);
        //MeowscaradaChoiceUI();

        // Wait for a button press
        while (!MeowbuttonPressed)
        {
            yield return null;
        }

        // Disable buttons immediately
        showMeowUI(false);

        if (reroll)
        {
            int secondRoll = 1;//prev player.DiceRollNumber
            Debug.Log("Second roll: " + secondRoll);
            player.StartMainMovement(secondRoll);
        }
        else
        {
            Debug.Log("Keeping first roll");
            player.StartMainMovement(firstRoll);
        }
    }




   
    //unused- deletion?
    public void MeowscaradaChoiceUI()
    {
        if (!isServer)
        {
            return;
        }
        MeowbuttonPressed = false;
        reroll = false;

        // Enable buttons when asking
        showMeowUI(true);
        
    }



    /* Buttons to pick whether player rerolls
     * only shown to play with client id = to turn 
     * 
     */
    public void RerollYesPressed()
    {
        ReRollToServer(true);
    }
    public void RerollNoPressed()
    {
        ReRollToServer(false);
    }
    [ServerRpc]
    private void ReRollToServer(bool isReroll, RPCInfo info = default)
    {
        if ((ushort)info.sender.id != (ushort)gameManger.currentPlayerTurn.value)
        {
            return;
        }
        reroll = isReroll;
        MeowbuttonPressed = true;
    }




    /* Buttons to show meow UI
     * all clients send local id to server
     * server picks which one to show based on its id
     */
    [ObserversRpc]
    public void showMeowUI(bool showUI)
    {
        PlayerID clientId = localPlayer.Value;
        ShowMeowUIServer(clientId, showUI);
    }
    [ServerRpc]
    private void ShowMeowUIServer(PlayerID target, bool showUI)
    {
        if ((ushort)gameManger.currentPlayerTurn.value == (ushort)target.id)
        {
            showMeowUITarget(target, showUI);
        }
    }
    [TargetRpc]
    public void showMeowUITarget(PlayerID target, bool showUI)
    {
        ReRollButton.gameObject.SetActive(showUI);
        DontReRollButton.gameObject.SetActive(showUI);
    }





}
