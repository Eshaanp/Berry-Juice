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
    public bool isMoving = false;
    public bool isDicePressed = false;

    [Header("Manager Scripts")]
    public TileEffects tileEffects;
    public GameManger gameManager;
    public PlayerTypes playerTypes;
    public WalkAnimations walkAnimations;
    public IdleAnimations idleAnimations;
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
        Hoopa,
        Oricorio,
        Raboot,
        Drifblim
    }
    [Header("Pokemon")]
    public Character character;

    public SyncList<Character> pickedCharacters = new SyncList<Character> ();

    [Header("Pokemon Sprites")]
    public GameObject Meowscarada;
    //public GameObject Jigglypuff;
    //public GameObject Luvdisc;
    //public GameObject Sligoo;
    public GameObject Patrat;
    //public GameObject Hoopa;
    //public GameObject Golisopod;
    public GameObject Victini;
    //public GameObject Oricorio;
    //public GameObject Raboot;
    //public GameObject Drifblim;



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

    [ObserversRpc]
    public void StartMainMovement(int roll)
    {
        StartCoroutine(MainMovement(roll));
    }
    [ObserversRpc]
    public void StartSlide(int roll)
    {
        StartCoroutine(MovementSlide(roll));
    }



    //Main Move, calls MovementSlide for movement
    public IEnumerator MainMovement(int tilesToMove)
    {
        Debug.Log("move");
        yield return MovementSlide(tilesToMove);
        isMoving = false;
        gameManager.playerReady();
    }

    //Moves the player model to tile
    public IEnumerator WalkToTile(GameObject targetTile)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(targetTile.transform.position.x, transform.position.y, targetTile.transform.position.z);

        walkAnimations.DetermineDirection(this, startPos, targetPos);
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
        idleAnimations.setIdleAnimation(this);
        tileEffects.CheckEffect(this);
        

    }

    public void Teleport(GameObject tile)
    {

     
        Vector3 pos = transform.position;
        pos.x = tile.transform.position.x;
        pos.z = tile.transform.position.z;
        transform.position = pos;
        currentTile = tile;


    }

    [ObserversRpc]
    public void SetUpCharacter(Character character)
    {
        Patrat.SetActive(false);
        this.character = character;
        switch (character)
        {
            case PlayerLogic.Character.Meowscarada:
                Meowscarada.SetActive(true);
                break;


            case PlayerLogic.Character.Victini:
                Victini.SetActive(true);
                break;

            default:
                Patrat.SetActive(true);
                break;

            


        }

        idleAnimations.setIdleAnimation(this);



    }



}
