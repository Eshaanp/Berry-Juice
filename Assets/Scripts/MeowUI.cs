using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PurrNet;

public class MeowUI : NetworkBehaviour
{



    [Header("Main Move")]
    public Button diceButton;
    //public bool isDicePressed;

    [Header("Meowscarada")]
    public Button ReRollButton;
    public Button DontReRollButton;
    public bool MeowbuttonPressed = false;
    public bool reroll = false;




    private void Start()
    {
        //this.gameObject.SetActive(false);
    }




    public IEnumerator ReRollChoice(PlayerLogic player, int firstRoll)
    {


        MeowscaradaChoiceUI();

        // Wait for a button press
        while (!MeowbuttonPressed)
        {
            yield return null;
        }

        // Disable buttons immediately
        hideUI("dontReroll");
        hideUI("reroll");

        if (reroll)
        {
            int secondRoll = 1;//prev player.DiceRollNumber
            Debug.Log("Second roll: " + secondRoll);
            yield return StartCoroutine(player.MainMovement(secondRoll));
        }
        else
        {
            Debug.Log("Keeping first roll");
            yield return StartCoroutine(player.MainMovement(firstRoll));
        }
    }




    //UI Management for Meowscarada re roll choice

    public void MeowscaradaChoiceUI()
    {
        if (!isServer)
        {
            return;
        }
        MeowbuttonPressed = false;
        reroll = false;

        // Enable buttons when asking
        showUI("reroll");
        showUI("dontReroll");
    }

    [ServerRpc]
    public void RerollYesPressed()
    {
        reroll = true;
        MeowbuttonPressed = true;
    }

    [ServerRpc]
    public void RerollNoPressed()
    {
        reroll = false;
        MeowbuttonPressed = true;
    }


    //UI for normal dice roll

    [ObserversRpc]
    public void showUI(string button)
    {

        //Debug.Log("ui shown");
        switch (button)
        {

            case "reroll":
                ReRollButton.gameObject.SetActive(true);
                break;
            case "dontReroll":
                DontReRollButton.gameObject.SetActive(true);
                break;

        }
        
    }

    [ObserversRpc]
    public void hideUI(string button)
    {
        switch (button)
        {

            case "reroll":
                ReRollButton.gameObject.SetActive(false);
                break;
            case "dontReroll":
                DontReRollButton.gameObject.SetActive(false);
                break;



        }

    }




}
