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
    public float moveSpeed = 5f;

    public bool isFreeCam;


    private void Start()
    {
        this.gameObject.SetActive(false);
    }


    void Update()
    {

        if (isFreeCam)
        {
            Vector3 movement = Vector3.zero;

            if (Keyboard.current.leftArrowKey.isPressed)
                movement.x -= 1f;

            if (Keyboard.current.rightArrowKey.isPressed)
                movement.x += 1f;

            if (Keyboard.current.upArrowKey.isPressed)
                movement.z += 1f;

            if (Keyboard.current.downArrowKey.isPressed)
                movement.z -= 1f;

            transform.position += movement.normalized * moveSpeed * Time.deltaTime;
        }


        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            //LockToCurrentPlayer();
        }
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            EnterFreeCam();
        }
    }


    [ObserversRpc]
    public void TurnOn(PlayerLogic Player1_Reference)
    {
        this.gameObject.SetActive(true);
        LockToCurrentPlayer(Player1_Reference);
        EnterFreeCam();
    }




    public void EnterFreeCam()
    {
        isFreeCam = true;
        this.transform.SetParent(parentCam.transform);
        Debug.Log("Entered Free Cam");
    }




    public void LockToCurrentPlayer(PlayerLogic currentPlayer)
    {
        //PlayerLogic currentPlayer = gameManger.GetCurrentPlayer();
        transform.SetParent(getCurrentPlayerCameraPosition(currentPlayer));
        transform.localPosition = Vector3.zero;
        isFreeCam = false;
        Debug.Log("camera locked to Player " +  currentPlayer.PlayerId);

        //transform.localRotation = Quaternion.identity;



    }






    private Transform getCurrentPlayerCameraPosition(PlayerLogic currentPlayer)
    {

        switch (currentPlayer.PlayerId)
        {
            case 1: return player1Cam;
            case 2: return player2Cam;
            case 3: return player3Cam;
            case 4: return player4Cam;
        }
        return null;

        

    }




}
