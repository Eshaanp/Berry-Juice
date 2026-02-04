using System.Collections;
using PurrNet;
using UnityEngine;
using static TileLogic;

public class TileEffects : NetworkBehaviour
{

    //Called to figure out the tile effect when a player ends on a tile 
    //no effects if play crosses finish line

    public GameManger gameManger;

    public void CheckEffect(PlayerLogic player)
    {
        //get tile tile of current player
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
                    player.StartSlide(1);
                    break;

                case TileType.SlideBackwards:
                    player.SlideSpriteChange(true);
                    player.StartSlide(-1);
                    //negativeTileEffect(false, player);
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
