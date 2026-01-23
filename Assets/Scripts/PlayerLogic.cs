using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

using static TileLogic;
using PurrNet;

public class PlayerLogic : NetworkBehaviour
{
    [Header("Player Information")]
    public int PlayerId;
    public int CurrentTileId = 0;
    public GameObject currentTile;


    public int moveNum = 1; 
    public float moveSpeed = 3f;
    public int points = 0; 


    [Header("Player States")]
    public bool skipTurn = false;
    public bool CrossedFinish = false;
    bool isMoving = false;
    public bool isDicePressed = false;

    [Header("Manager Scripts")]
    public TileEffects tileEffects;
    public GameManger gameManager;
    public PlayerTypes playerTypes;
    //public UIManager uIManager;



    
    public enum Character
    {
        Patrat,
        Jigglypuff,
        Sligoo,
        Meowscarada,
        Luvdisc,
        Victini,
        Golisopod,
        Hoopa
    }
    [Header("Pokemon")]
    public Character character;

    public SyncList<Character> pickedCharacters = new SyncList<Character> ();


   


    //Puts character on first tile
    void Start()
    {
        if (currentTile != null)
        {
            Vector3 pos = transform.position;
            pos.x = currentTile.transform.position.x;
            pos.z = currentTile.transform.position.z;
            transform.position = pos;
        }
    }


    //will simplify/fix dice later
    //calls UIManager for dice button
    /*
    public IEnumerator DiceRoll()
    {

        if (!isServer)
        {
            yield break;
        }
        Debug.Log("test");
        isDicePressed = false;
        ShowDiceUIRpc();

        while (!isDicePressed)
        {
            yield return null;
        }
        
        HideDiceUIRpc();
        StartCoroutine(MainMovement(moveNum));
    }

    [ObserversRpc]
    public void ShowDiceUIRpc()
    {
        if (uIManager == null)
            uIManager = UIManager.Instance;

        Debug.Log("ShowDiceUIRpc running"); // this should appear on all clients
        uIManager.showUI("dice");
    }

    [ObserversRpc]
    private void HideDiceUIRpc()
    {
        uIManager.hideUI("dice");
    }

    [ServerRpc]
    public void RollDiceServerRpc()
    {
        isDicePressed = true;
    }

    public int DiceRollNumber()
    {
        return moveNum;
    }*/
    
    public void test()
    {
        Debug.Log("testing mov");
    }

    //Main Move, calls MovementSlide for movement
    public IEnumerator MainMovement(int tilesToMove)
    {
        Debug.Log("move");
        yield return MovementSlide(tilesToMove);
        isMoving = false;
        gameManager.EndTurn();
    }

    //Moves the player model to tile
    public IEnumerator WalkToTile(GameObject targetTile)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(targetTile.transform.position.x, transform.position.y, targetTile.transform.position.z);

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
    }


    //This is general movement  
    //Will remove to simplify later 
    public IEnumerator MovementSlide(int tilesToMove)
    {
        //Debug.Log("applying slide");

        Debug.Log(" moving by: " +  tilesToMove);

        if (currentTile == null)
        {
            yield break;
        }
        currentTile.GetComponent<TileLogic>().setPlayerOffTile(this);
        isMoving = true;

        int steps = Mathf.Abs(tilesToMove);
        bool movingForward = tilesToMove > 0;

        for (int i = 0; i < steps; i++)
        {
            TileLogic tileLogic = currentTile.GetComponent<TileLogic>();
            if (tileLogic.tileType == TileLogic.TileType.EndTile)
            {
                break;
            }

            GameObject nextTile = movingForward ? tileLogic.nextTile : tileLogic.prevTile;

            if (nextTile == null)
            {
                break;
            }

            yield return StartCoroutine(WalkToTile(nextTile));

            //For characters that activate effect during movement
            if (i != 0)
            {
                playerTypes.CheckCharacterDuringRole(this);
            }

            currentTile = nextTile;
            CurrentTileId = currentTile.GetComponent<TileLogic>().id;

            //Debug.Log("Visited tile " + CurrentTileId);
        }
        currentTile.GetComponent<TileLogic>().setPlayerOnTile(this);
        tileEffects.CheckEffect(this);
        

    }





}
