using PurrNet;
using UnityEditor.Rendering;
using UnityEngine;
using System.Collections;

public class CameraManager : NetworkIdentity
{

    public GameManger gameManger;
    public CameraControl cam1;
    public Camera MainCam;




    [ServerRpc]
    public void LockToCurrentPlayer()
    {
        //GetLockPlayerCamera(gameManger.numOfPlayers, gameManger.GetCurrentPlayer());
        cam1.MainCameraInterface(false, gameManger.GetCurrentPlayer());
    }

    [ServerRpc]
    public void startFreeCamera()
    {

        cam1.MainCameraInterface(true, gameManger.GetCurrentPlayer());

    }




    [ObserversRpc]
    public void CameraSetUp()
    {
        MainCam.gameObject.SetActive(false);
        PlayerID clientID = localPlayer.Value;
        cam1.gameObject.SetActive(true);
        cam1.TurnOn(gameManger.GetCurrentPlayer());

    }
}
