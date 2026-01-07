using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using static TileLogic;

public class PlayerLogic : MonoBehaviour
{

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

    [Header("Manager Scripts")]
    public TileEffects tileEffects;
    public GameManger gameManager;
    public PlayerTypes playerTypes;
    public UIManager uIManager;



    
    public enum Character
    {
        Patrat,
        Jigglypuff,
        Sligoo,
        Meowscarada,
        Luvdisc
    }
    [Header("Pokemon")]
    public Character character;

    //[Header("Character Effect States")]



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

    void Update()
    {
        //Check player turn then space to move for current tests
        //amount of spaces is moveNum but will be replaced
       /* if (!gameManager.isPlayersTurn(PlayerId))
        {
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isMoving)
        {
            StartCoroutine(MoveTilesCoroutine(moveNum));

        }*/
    }

    //will simplify/fix dice later
    public IEnumerator DiceRoll()
    {
        uIManager.isDicePressed = false;
        uIManager.diceButton.gameObject.SetActive(true);

        while (!uIManager.isDicePressed)
        {
            yield return null;
        }

        StartCoroutine(MainMovement(moveNum));
    }

    public int DiceRollNumber()
    {
        return moveNum;
    }
    

    //Main Move, calls MovementSlide for movement
    public IEnumerator MainMovement(int tilesToMove)
    {
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
