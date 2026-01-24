using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PurrNet;
public class DriftUI : NetworkBehaviour
{
    [Header("Main Move")]
    public Button diceButton;
    //public bool isDicePressed;

    [Header("Driflim")]
    public Button DoubleButton;
    public Button DontDoubleButton;
    public bool driftButtonPressed = false;
    public bool doubleRoll = false;




    private void Start()
    {
        //this.gameObject.SetActive(false);
    }




    public IEnumerator DoubleForTrip(PlayerLogic player, int firstRoll)
    {


        DriftChoiceUI();

        // Wait for a button press
        while (!driftButtonPressed)
        {
            yield return null;
        }

        // Disable buttons immediately
        hideUI();

        if (doubleRoll)
        {
            int doubledRoll = firstRoll * 2;
            Debug.Log("double roll: " + doubledRoll);
            player.skipTurn = true; // trip player 
            yield return StartCoroutine(player.MainMovement(doubledRoll));
        }
        else
        {
            Debug.Log("Keeping first roll");
            yield return StartCoroutine(player.MainMovement(firstRoll));
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
        showUI();
        
    }

    [ServerRpc]
    public void DoubleYesPressed()
    {
        doubleRoll = true;
        driftButtonPressed = true;
    }

    [ServerRpc]
    public void DoubleNoPressed()
    {
        doubleRoll = false;
        driftButtonPressed = true;
    }


    //UI for normal dice roll

    [ObserversRpc]
    public void showUI()
    {
        DoubleButton.gameObject.SetActive(true);
        DontDoubleButton.gameObject.SetActive(true);

    }

    [ObserversRpc]
    public void hideUI()
    {
        DoubleButton.gameObject.SetActive(false);
        DontDoubleButton.gameObject.SetActive(false);
       

    }



}
