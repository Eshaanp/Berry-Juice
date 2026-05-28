using UnityEngine;

public class TileParent : MonoBehaviour
{


    public bool RandomSet = false;



    public void SetAllTiles()
    {
        foreach (TileLogic tile in GetComponentsInChildren<TileLogic>())
        {
            if (RandomSet == true)
            {
                tile.DetermineTileType();
                tile.SetTileForAllPlayers(tile.tileType);
            }
            else
            {
                tile.SetTileForAllPlayers(TileLogic.TileType.normalTile);
            }
        }
    }




}
