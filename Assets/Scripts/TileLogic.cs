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

    //public bool RandomSet = false;

    public enum TileType { 
        normalTile,
        StartTile,
        EndTile,
        SlideForward,
        SlideBackwards,
        TripTile,
        PenaltyTile, 
        PointTile,
        CardTile

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
 
        
        //SetTileForAllPlayers(tileType);
        //tileRenderer = GetComponent<Renderer>();
        //ApplyTileColor();
    }


    
    public void DetermineTileType()
    {
        //start and End tiles never change
        if (tileType == TileType.StartTile || tileType == TileType.EndTile)
            return;

        float roll = Random.value; // 0.0 to 1.0

        float changeChance = tileType == TileType.normalTile ? 0.10f
                           : tileType == TileType.CardTile ? 0.15f
                           : 0.30f;

        if (roll >= changeChance)
            return;

        //Build the pool of tiles this type can change into
        TileType[] changeable = tileType == TileType.CardTile
            ? new TileType[]
              {
              TileType.normalTile,
              TileType.SlideForward,
              TileType.SlideBackwards,
              TileType.TripTile,
              TileType.PenaltyTile,
              TileType.PointTile
              }
            : new TileType[]
              {
              TileType.SlideForward,
              TileType.SlideBackwards,
              TileType.TripTile,
              TileType.PenaltyTile,
              TileType.PointTile,
              TileType.CardTile
              };

        //Pick a random type from the pool (excluding the current one)
        TileType[] options = System.Array.FindAll(changeable, t => t != tileType);
        tileType = options[Random.Range(0, options.Length)];
        //SetTileForAllPlayers(tileType);
    }

    [ObserversRpc]
    public void SetTileForAllPlayers(TileType tile)
    {
        Debug.Log("Set tile");
        tileType = tile;
        tileRenderer = GetComponent<Renderer>();
        ApplyTileColor();
    }

    //Player object passed in, compares id to check if on tile
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

            case TileType.PenaltyTile:
                tileRenderer.material.color = Color.brown;
                break;
            case TileType.SlideBackwards:
                tileRenderer.material.color = Color.purple;
                break;
            case TileType.PointTile:
                tileRenderer.material.color = Color.orange;
                break;
            case TileType.CardTile:
                tileRenderer.material.color = Color.black; 
                break;
        }
    }


}
