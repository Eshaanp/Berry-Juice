using PurrNet;
using UnityEditor.Rendering;
using UnityEngine;

public class CameraManager : NetworkIdentity
{

    public GameManger gameManger;
    public CameraControl cam1;
    public CameraControl cam2;
    public CameraControl cam3;
    public CameraControl cam4;
    public Camera MainCam;




    [ServerRpc]
    public void LockToCurrentPlayer()
    {
        //GetLockPlayerCamera(gameManger.numOfPlayers, gameManger.GetCurrentPlayer());
        cam1.LockToCurrentPlayer(gameManger.GetCurrentPlayer());
    }

    [ServerRpc]
    public void startFreeCamera()
    {
        //GetFreePlayerCamera(gameManger.numOfPlayers);

        cam1.EnterFreeCam();

    }

    /*
    [ObserversRpc]
    public void GetLockPlayerCamera(int playerCount, PlayerLogic currentPlayer)
    {
        PlayerID clientID = localPlayer.Value;

        if((ushort)clientID.id == (ushort)1)
        {
            cam1.LockToCurrentPlayer(currentPlayer);
        }
        if ((ushort)clientID.id == (ushort)2)
        {
            cam2.LockToCurrentPlayer(currentPlayer);
        }
        if ((ushort)clientID.id == (ushort)3 && playerCount > 2)
        {
            cam3.LockToCurrentPlayer(currentPlayer);
        }
        if ((ushort)clientID.id == (ushort)4 && playerCount == 4)
        {
            cam4.LockToCurrentPlayer(currentPlayer);
        }
    }

    [ObserversRpc]
    public void GetFreePlayerCamera(int playerCount)
    {
        PlayerID clientID = localPlayer.Value;

        if ((ushort)clientID.id == (ushort)1)
        {
            cam1.EnterFreeCam();
        }
        if ((ushort)clientID.id == (ushort)2)
        {
            cam2.EnterFreeCam();
        }
        if ((ushort)clientID.id == (ushort)3 && playerCount > 2)
        {
            cam3.EnterFreeCam();
        }
        if ((ushort)clientID.id == (ushort)4 && playerCount == 4)
        {
            cam4.EnterFreeCam();
        }
    }*/


    [ObserversRpc]
    public void CameraSetUp()
    {
        MainCam.gameObject.SetActive(false);
        PlayerID clientID = localPlayer.Value;
        cam1.gameObject.SetActive(true);
        cam1.TurnOn(gameManger.GetCurrentPlayer());

        /*
        switch (clientID.id)
        {
            case (ushort)1:
                cam1.gameObject.SetActive(true);
                cam1.TurnOn(gameManger.player1);
                break;
            case (ushort)2:
                cam2.gameObject.SetActive(true);
                cam2.TurnOn(gameManger.player1);
                break;
            case (ushort)3:
                cam3.gameObject.SetActive(true);
                cam3.TurnOn(gameManger.player1);
                break;
            case (ushort)4:
                cam4.gameObject.SetActive(true);
                cam4.TurnOn(gameManger.player1);
                break;
        }*/
    }
}
