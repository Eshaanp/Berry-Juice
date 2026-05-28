using UnityEngine;
using System.Collections;
using PurrNet;

public class AI_Manager : NetworkBehaviour
{

    public GameManger gameManager;



    public IEnumerator AI_Turn()
    {
        if (!isServer)
        {
            yield break;
        }

        if (gameManager.GetCurrentPlayer().character == PlayerLogic.Character.Hoopa || gameManager.GetCurrentPlayer().character == PlayerLogic.Character.Oricorio)
        {

        }
        






    }







}
