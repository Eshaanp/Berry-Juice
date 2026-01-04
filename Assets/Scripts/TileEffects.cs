using UnityEngine;
using static TileLogic;

public class TileEffects : MonoBehaviour
{

    //Tile effects and stuff
    public GameManger gameManger;

    public void CheckEffect(PlayerLogic player)
    {
        Debug.Log("check effect");
        TileLogic tile = player.currentTile.GetComponent<TileLogic>();


        if (player.CrossedFinish == false) { 
            switch (tile.tileType)
            {
                case TileType.StartTile:
                    break;

                case TileType.EndTile:
                    player.CrossedFinish = true;
                    gameManger.updateScore(gameManger.turn);
                    break;

                case TileType.SlideForward:
                    StartCoroutine(player.ApplySlide(1));
                    break;

                case TileType.TripTile:
                    player.skipTurn = true;
                    break;

                case TileType.PitfallTile:
                    gameManger.updateScore(1);
                    break;
            }
        }
    }
}
