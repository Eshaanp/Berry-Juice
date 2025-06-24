using NUnit.Framework;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    
    private void Awake() {
        BuildBoard();
    }

    private void BuildBoard() {

        Vector3[] coordinates = new Vector3[8];
        coordinates[0] = new Vector3(0, 0, 0);coordinates[1] = new Vector3(2.5f, 0, 0);coordinates[2] = new Vector3(5f, 0, 0);
        coordinates[3] = new Vector3(0, 0, 2.5f);                                 coordinates[4] = new Vector3(5f, 0, 2.5f);
        coordinates[5] = new Vector3(0, 0, 5f);coordinates[6] = new Vector3(2.5f, 0, 5f);coordinates[7] = new Vector3(5f, 0, 5f);

        GameObject[] tiles = new GameObject[8];
        Color[] colors = new Color[] { Color.teal, Color.purple, Color.pink };

        for (int i = 0; i < coordinates.GetLength(0); i++) {
            tiles[i] = Instantiate(tile, coordinates[i], Quaternion.identity);
            tiles[i].GetComponentInChildren<MeshRenderer>().material.color = colors[i % 3];
        }


    }
}
