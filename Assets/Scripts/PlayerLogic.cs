using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerLogic : MonoBehaviour
{


    public int PlayerId;
    //public bool isTurn = false; 

    public int CurrentTileId = 0;
    public GameObject currentTile;

    public int moveNum = 1; 
    public float moveSpeed = 3f;

    bool isMoving = false;

    public TileEffects tileEffects;

    public GameManger gameManager;


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
        if (!gameManager.isPlayersTurn(PlayerId))
        {
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isMoving)
        {
            StartCoroutine(MoveTilesCoroutine(moveNum));

        }
    }


    //Moves player on first dice roll
    public IEnumerator MoveTilesCoroutine(int tilesToMove)
    {
        if (currentTile == null)
        {
            yield break;
        }

        isMoving = true;

        int steps = Mathf.Abs(tilesToMove);
        bool movingForward = tilesToMove > 0;


        for (int i = 0; i < steps; i++)
        {
            TileLogic tileLogic = currentTile.GetComponent<TileLogic>();

            GameObject nextTile = movingForward
                ? tileLogic.nextTile
                : tileLogic.prevTile;

            if (nextTile == null)
            {
                break;
            }

            //this just moves the model
            yield return StartCoroutine(WalkToTile(nextTile));

            currentTile = nextTile;
            CurrentTileId = currentTile.GetComponent<TileLogic>().id;

            //Debug.Log("Visited tile " + CurrentTileId);
        }

        //checks the effect of landed tile
        tileEffects.CheckEffect(this);
        isMoving = false;
        gameManager.NextTurn();
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

    //This is movement again, this is basically just the movement method again without NextTurn and stuff. 
    //Will remove to simplify later 
    public IEnumerator ApplySlide(int tilesToMove)
    {
        Debug.Log("applying slide");

        if (currentTile == null)
        {
            yield break;
        }

        isMoving = true;

        int steps = Mathf.Abs(tilesToMove);
        bool movingForward = tilesToMove > 0;

        for (int i = 0; i < steps; i++)
        {
            TileLogic tileLogic = currentTile.GetComponent<TileLogic>();

            GameObject nextTile = movingForward
                ? tileLogic.nextTile
                : tileLogic.prevTile;

            if (nextTile == null)
            {
                break;
            }

            yield return StartCoroutine(WalkToTile(nextTile));

            currentTile = nextTile;
            CurrentTileId = currentTile.GetComponent<TileLogic>().id;

            //Debug.Log("Visited tile " + CurrentTileId);
        }

        //
        tileEffects.CheckEffect(this);

    }


}
