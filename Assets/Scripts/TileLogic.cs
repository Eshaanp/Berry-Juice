using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class TileLogic : MonoBehaviour
{

    public int id;
    public GameObject prevTile;
    public GameObject nextTile;


    /*
    public bool isStartTile;
    public bool isEndTile;
    public bool isSlideForward;
    public bool isTripTile;
    */


    public bool isPlayer1OnTile = false;
    public bool isPlayer2OnTile = false;
    public bool isPlayer3OnTile = false;
    public bool isPlayer4OnTile = false;


    Renderer tileRenderer;
    
    //Tile Types
    public enum TileType { 
        normalTile,
        StartTile,
        EndTile,
        SlideForward,
        TripTile,
        PitfallTile
    }

    public TileType tileType;

    void Start()
    {
        tileRenderer = GetComponent<Renderer>();
        ApplyTileColor();
    }

    
    void Update()
    {
        
    }




    public void setPlayerOnTile(PlayerLogic player)
    {
        switch (player.PlayerId)
        {
            case 1:
                isPlayer1OnTile = true;
                break;

            case 2:
                isPlayer2OnTile = true;
                break;

            case 3:
                isPlayer3OnTile = true;
                break;

            case 4:
                isPlayer4OnTile = true;
                break;

        }
    }
    public void setPlayerOffTile(PlayerLogic player)
    {
        switch (player.PlayerId)
        {
            case 1:
                isPlayer1OnTile = false;
                break;

            case 2:
                isPlayer2OnTile = false;
                break;

            case 3:
                isPlayer3OnTile = false;
                break;

            case 4:
                isPlayer4OnTile = false;
                break;

        }
    }


    void ApplyTileColor()
    {
        switch (tileType)
        {
            case TileType.StartTile:
                tileRenderer.material.color = Color.green;
                break;

            case TileType.EndTile:
                tileRenderer.material.color = Color.blue;
                break;

            case TileType.SlideForward:
                tileRenderer.material.color = Color.yellow;
                break;

            case TileType.TripTile:
                tileRenderer.material.color = Color.red;
                break;

            case TileType.PitfallTile:
                tileRenderer.material.color = Color.brown;
                break;
        }
    }


}
