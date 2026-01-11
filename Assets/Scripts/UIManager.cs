using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    //General Script for managing UI DURING the game


    [Header("Main Move")]
    public Button diceButton;
    public bool isDicePressed;

    [Header("Meowscarada")]
    public Button ReRollButton;
    public Button DontReRollButton;
    public bool MeowbuttonPressed = false;
    public bool reroll = false;




    private void Start()
    {
        this.gameObject.SetActive(false);
    }


    //UI Management for Meowscarada re roll choice
    public void MeowscaradaChoiceUI()
    {
        MeowbuttonPressed = false;
        reroll = false;

        // Enable buttons when asking
        ReRollButton.gameObject.SetActive(true);
        DontReRollButton.gameObject.SetActive(true);
    }
    public void RerollYesPressed()
    {
        reroll = true;
        MeowbuttonPressed = true;
    }
    public void RerollNoPressed()
    {
        reroll = false;
        MeowbuttonPressed = true;
    }


    //UI for normal dice roll
    public void RollDice()
    {
        isDicePressed = true;
        diceButton.gameObject.SetActive(false);

    }








}
