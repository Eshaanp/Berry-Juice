using PurrNet;
using TMPro;
using UnityEngine;

public class MainUI : NetworkBehaviour
{
    [Header("Player UI Elements")]
    //Player1 UI
    public GameObject Player1Icon;
    public TextMeshProUGUI Player1Name;
    public TextMeshProUGUI Player1Points;
    public TextMeshProUGUI Player1CardCount;

    //Player2 UI
    public GameObject Player2Icon;
    public TextMeshProUGUI Player2Name;
    public TextMeshProUGUI Player2Points;
    public TextMeshProUGUI Player2CardCount;

    //Player3 UI
    public GameObject Player3Icon;
    public TextMeshProUGUI Player3Name;
    public TextMeshProUGUI Player3Points;
    public TextMeshProUGUI Player3CardCount;

    //Player4 UI
    public GameObject Player4Icon;
    public TextMeshProUGUI Player4Name;
    public TextMeshProUGUI Player4Points;
    public TextMeshProUGUI Player4CardCount;

    [Header("")]
    public GameManger GameManger;


    [ObserversRpc]
    public void UpdatePointUI()
    {
        Player1Points.text = GameManger.Player1Score + " Berry";
        Player2Points.text = GameManger.Player2Score + " Berry";
        Player3Points.text = GameManger.Player3Score + " Berry";
        Player4Points.text = GameManger.Player4Score + " Berry";


    }

    [ObserversRpc]
    public void UpdateNameUI()
    {
        Player1Name.text = "Player 1";
        Player2Name.text = "Player 2";
        Player3Name.text = "Player 3";
        Player4Name.text = "Player 4";

    }


    [ObserversRpc]
    public void UpdateIcon()
    {

    }























}
