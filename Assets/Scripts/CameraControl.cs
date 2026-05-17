using PurrNet;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : NetworkIdentity
{
    public GameObject parentCam;
    public int cameraID;
    public GameManger gameManger;
    public Transform player1Cam;
    public Transform player2Cam;
    public Transform player3Cam;
    public Transform player4Cam;

    public float followSpeed = 10f;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }


    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LockToPlayer();
        }
    }


        [ObserversRpc]
    public void CameraSetUp()
    {
        PlayerID clientID = localPlayer.Value;
        if( (ushort) cameraID == (ushort) clientID.id)
        {
            this.gameObject.SetActive(true);
        }
    }




    public void EnterFreeCam()
    {

    }


    public void ExitFreeCam()
    {

    }


    public void LockToPlayer()
    {
        PlayerLogic currentPlayer = gameManger.GetCurrentPlayer();
        transform.SetParent(player1Cam);

        // Reset local offset if desired
        transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.identity;



    }



}
