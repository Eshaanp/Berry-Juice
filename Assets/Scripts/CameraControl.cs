using PurrNet;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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


    }


    [ObserversRpc]
    public void TurnOn(PlayerLogic Player1_Reference)
    {
        
        StartCoroutine(changeCamera(false, Player1_Reference));
        //EnterFreeCam();
    }






    [ObserversRpc]
    public void MainCameraInterface(bool freeCam, PlayerLogic currentPlayer)
    {
        StartCoroutine(changeCamera(freeCam, currentPlayer));

    }

    public IEnumerator changeCamera(bool freeCam, PlayerLogic currentPlayer)
    {

        Debug.Log("Locked to Player: " + currentPlayer.PlayerId);
        yield return StartCoroutine(MoveToLocalZeroXZ(freeCam, 1f, currentPlayer));
        

    }


    public IEnumerator MoveToLocalZeroXZ(bool freeCam, float duration, PlayerLogic currentPlayer)
    {

        Vector3 target;

        if (freeCam)
        {
            this.transform.SetParent(parentCam.transform);
            target = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
            isFreeCam = true;
        } else {
            transform.SetParent(getCurrentPlayerCameraPosition(currentPlayer));
            target = new Vector3(0, 0, 0);
            isFreeCam = false;
        }

        Vector3 start = transform.localPosition;
        
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        transform.localPosition = target;
    }



    private Transform getCurrentPlayerCameraPosition(PlayerLogic currentPlayer)
    {
        Debug.Log("Transform: " + currentPlayer.PlayerId);
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
