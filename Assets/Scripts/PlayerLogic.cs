using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

using static TileLogic;
using PurrNet;
using UnityEngine.ProBuilder;

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

    //pokemon model exceptions
    public GameObject minunSprite;
    public GameObject plusleSprite;

    
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
    public GameObject Jigglypuff;
    public GameObject Luvdisc;
    public GameObject Sligoo;
    public GameObject Patrat;
    public GameObject Hoopa;
    public GameObject Golisopod;
    public GameObject Victini;
    public GameObject Oricorio;
    public GameObject Raboot;
    public GameObject Drifblim;



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
        gameManager.cameraManager.LockToCurrentPlayer(); //camera
        StartCoroutine(MainMovement(roll));
    }
    [ObserversRpc]
    public void StartSlide(int roll)
    {
        StartCoroutine(SlideMovement(roll));
    }
    [ObserversRpc]
    public void StartTeleport(GameObject tile)
    {
        Teleport(tile);
    }




    //Main Move, calls MovementSlide for movement
    public IEnumerator MainMovement(int tilesToMove)
    {
        Debug.Log("move");
        yield return MovementSlide(tilesToMove);
        isMoving = false;
        gameManager.playerReady();
    }

    public IEnumerator SlideMovement(int tilesToMove)
    {
        Debug.Log("slide");
        yield return MovementSlide(tilesToMove);
        isMoving = false;
        SlideSpriteChange(false);
    }


    //Moves the player model to tile
    public IEnumerator WalkToTile(GameObject targetTile)
    {
        Debug.Log("Target tile- " + targetTile);
      
        Vector3 startPos = transform.position;
        Vector3 targetPos = targetTile.transform.position;
        targetPos.y = startPos.y; //lock Y for board movement

        float distance = Vector3.Distance(startPos, targetPos);
        if (distance < 0.001f)
            yield break;

        float duration = distance / moveSpeed;
        float elapsed = 0f;

        
        walkAnimations.DetermineDirection(this, startPos, targetPos);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            transform.position = Vector3.Lerp(startPos, targetPos, t);

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
        //Debug.Log("Moving Forwards: " + movingForward);

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
            //Debug.Log("Next Tile: " + nextTile.name);
            yield return StartCoroutine(WalkToTile(nextTile));

            //Debug.Log("Past Walking");
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

    [ObserversRpc]
    public void Teleport(GameObject tile)
    {
        Vector3 pos = transform.position;
        pos.x = tile.transform.position.x;
        pos.z = tile.transform.position.z;
        transform.position = pos;
        currentTile = tile;
        tile.GetComponent<TileLogic>().setPlayerOnTile(this);
        CurrentTileId = tile.GetComponent<TileLogic>().id;
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

            case PlayerLogic.Character.Sligoo:
                Sligoo.SetActive(true);
                break;

            case PlayerLogic.Character.Golisopod:
                Golisopod.SetActive(true);
                break;

            case PlayerLogic.Character.Drifblim:
                Drifblim.SetActive(true);
                break;

            case PlayerLogic.Character.Jigglypuff:
                Jigglypuff.SetActive(true);
                break;

            case PlayerLogic.Character.Oricorio:
                Oricorio.SetActive(true);
                break;

            case PlayerLogic.Character.Raboot:
                Raboot.SetActive(true);
                break;

            case PlayerLogic.Character.Hoopa:
                Hoopa.SetActive(true);
                break;

            case PlayerLogic.Character.Luvdisc:
                Luvdisc.SetActive(true);
                break;

            default:
                Patrat.SetActive(true);
                break;

        }

        idleAnimations.setIdleAnimation(this);

    }


    [ObserversRpc]
    public void TurnOffAllSprites()
    {
        Meowscarada.SetActive(false);
        Victini.SetActive(false);
        Sligoo.SetActive(false);
        Golisopod.SetActive(false);
        Patrat.SetActive(false);
        Drifblim.SetActive(false);
        Jigglypuff.SetActive(false);
        Oricorio.SetActive(false);
        Raboot.SetActive(false);
        Hoopa.SetActive(false);
        Luvdisc.SetActive(false);

    }



    public GameObject getCharacterSprite()
    {
        switch (character)
        {
            case PlayerLogic.Character.Meowscarada:
                return Meowscarada;
                
            case PlayerLogic.Character.Victini:
                return Victini;

            case PlayerLogic.Character.Sligoo:
                return Sligoo;

            case PlayerLogic.Character.Golisopod:
                return Golisopod;

            case PlayerLogic.Character.Drifblim:
                return Drifblim;

            case PlayerLogic.Character.Jigglypuff:
                return Jigglypuff;

            case PlayerLogic.Character.Oricorio:
                return Oricorio;

            case PlayerLogic.Character.Raboot:
                return Raboot;

            case PlayerLogic.Character.Hoopa:
                return Hoopa;

            case PlayerLogic.Character.Luvdisc:
                return Luvdisc;

            default:
                return Patrat;
        }
        
    }

    //start = false sets back to normal
    //doesnt work with minun/plusle
    [ObserversRpc]
    public void SlideSpriteChange(bool start)
    {
        GameObject sprite = getCharacterSprite();

        if (!start)
        {
            //sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
            moveSpeed = 5;
        }
        /*else if (sprite = Oricorio)
        {
            plusleSprite.GetComponent<SpriteRenderer>().color = new Color(1f, .5f, .5f);
            minunSprite.GetComponent<SpriteRenderer>().color = new Color(1f, .5f, .5f);
            moveSpeed = 2;
        }*/
        else 
        {
            //sprite.GetComponent<SpriteRenderer>().color = new Color(1f, .5f, .5f);
            moveSpeed = 2;

        }

    }




}
