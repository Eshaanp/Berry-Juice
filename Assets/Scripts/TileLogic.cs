using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using PurrNet;

public class TileLogic : NetworkBehaviour
{
    //Script to define tiles 

    [Header("Tile Information")]
    public int id;
    public GameObject prevTile;
    public GameObject nextTile;

    [Header("Player On Tile")]
    public bool isPlayer1OnTile = false;
    public bool isPlayer2OnTile = false;
    public bool isPlayer3OnTile = false;
    public bool isPlayer4OnTile = false;


    public enum TileType { 
        normalTile,
        StartTile,
        EndTile,
        SlideForward,
        TripTile,
        PitfallTile
    }
    [Header("Type of Tile")]
    public TileType tileType;


    Renderer tileRenderer;



    private void Start()
    {
        //tileRenderer = GetComponent<Renderer>();
        //ApplyTileColor();
    }

    protected override void OnSpawned()
    {
        base.OnSpawned();
        tileRenderer = GetComponent<Renderer>();
        ApplyTileColor();
    }

    

    // Player object passed in, compares id to check if on tile
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

    //checks player off tile
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


    //Change color depending on type
    
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
