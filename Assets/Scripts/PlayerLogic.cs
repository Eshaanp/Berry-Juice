using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerLogic : MonoBehaviour
{
    public int PlayerId;

    public int CurrentTileId = 0;
    public GameObject currentTile;

    public int moveNum = 1; 
    public float moveSpeed = 3f;

    bool isMoving = false;

    void Start()
    {
        if (currentTile != null)
        {
            Vector3 pos = transform.position;
            pos.x = currentTile.transform.position.x;
            pos.z = currentTile.transform.position.z;
            transform.position = pos;
        }
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isMoving)
        {
            StartCoroutine(MoveTilesCoroutine(moveNum));
        }
    }

    IEnumerator MoveTilesCoroutine(int tilesToMove)
    {
        if (currentTile == null)
        {
            yield break;
        }

        isMoving = true;

        int steps = Mathf.Abs(tilesToMove);
        bool movingForward = tilesToMove > 0;

        for (int i = 0; i < steps; i++)
        {
            TileLogic tileLogic = currentTile.GetComponent<TileLogic>();

            GameObject nextTile = movingForward
                ? tileLogic.nextTile
                : tileLogic.prevTile;

            if (nextTile == null)
            {
                break;
            }

            yield return StartCoroutine(WalkToTile(nextTile));

            currentTile = nextTile;
            CurrentTileId = currentTile.GetComponent<TileLogic>().id;

            Debug.Log("Visited tile " + CurrentTileId);
        }

        isMoving = false;
    }

    IEnumerator WalkToTile(GameObject targetTile)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(targetTile.transform.position.x, transform.position.y, targetTile.transform.position.z);

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
    }


}
