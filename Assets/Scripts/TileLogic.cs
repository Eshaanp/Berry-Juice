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
