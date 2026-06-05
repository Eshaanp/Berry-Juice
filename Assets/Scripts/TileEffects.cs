using System.Collections;
using PurrNet;
using UnityEngine;
using static TileLogic;

public class TileEffects : NetworkBehaviour
{

    //Called to figure out the tile effect when a player ends on a tile 
    //no effects if play crosses finish line

    public GameManger gameManger;

    public IEnumerator CheckEffect(PlayerLogic player)
    {
        //get tile tile of current player
        TileLogic tile = player.currentTile.GetComponent<TileLogic>();

        
        if (player.CrossedFinish == false) { 
            switch (tile.tileType)
            {
                case TileType.StartTile:
                    yield break;

                case TileType.EndTile:
                    player.CrossedFinish = true;
                    gameManger.updateScore(gameManger.GetCurrentPlayer() ,gameManger.turn);
                    yield break; 

                case TileType.SlideForward:
                    yield return StartCoroutine(player.SlideMovement(1));
                    yield break; 

                case TileType.SlideBackwards:
                    player.SlideSpriteChange(true);
                    yield return StartCoroutine(player.SlideMovement(-1));
                    //negativeTileEffect(false, player);
                    yield break; 

                case TileType.TripTile:
                    player.skipTurn = true;
                    yield break; 

                case TileType.PenaltyTile:
                    gameManger.updateScore(gameManger.GetCurrentPlayer() , -1);
                    yield break; 

                case TileType.PointTile:
                    gameManger.updateScore(gameManger.GetCurrentPlayer(),1);
                    yield break; 
                case TileType.CardTile: 
                    int currentPlayerID = gameManger.GetCurrentPlayer().PlayerId;
                    gameManger.cardServerManager.giveCardTargetPlayer(currentPlayerID);
                    yield break; 
            }
        }
    }



}
