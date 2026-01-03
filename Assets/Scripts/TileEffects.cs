using UnityEngine;

public class TileEffects : MonoBehaviour
{

    //Tile effects and stuff

    public void CheckEffect(PlayerLogic player)
    {
        Debug.Log("check effect");
        TileLogic tile = player.currentTile.GetComponent<TileLogic>();

        if (tile.isSlideForward)
        {

            //tile.isSlideForward = false;
            //player.ApplySlide(1);
            StartCoroutine(player.ApplySlide(1));
        }
    }
}
