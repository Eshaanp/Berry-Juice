using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


//Spawns the map, manages calls to get information about the map (tile identity, next tile, etc)

public class TileSpawner : MonoBehaviour
{

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private string mapName;
    [SerializeField] private string mapPath;

    private Dictionary<string, Tile> tileData = new Dictionary<string, Tile>();
    private List<string> homeIDs = new List<string>();

    private void Awake() {
        //Read from csv file using map name or path or something, hopefully relative pathing works
        mapName = "Maps/PracticeRange";
        BuildBoard(mapName);
    }

    /**
     * 1. Read csv and parse Tile Id, coordinates, tile type, next tiles (ID), previous tiles (ID)
     * 
     * 2. Store tiles into the instantiated Map, store this map in the mapData field
     * 
     * 3. Instantiate all the tiles based on the parsed information, either during construction or after if that's easier (proabbly not)
     */
    private void BuildBoard(string csvDump) {

        float tileSize = 2f;
        float tileGap = .2f;

        var fullData = Resources.Load <TextAsset> (csvDump);
        var tileSplit = fullData.text.Split('\n');

        for (int i = 0; i < tileSplit.Length - 1; i++) {
            string tile = tileSplit[i];
           
            string[] pieces = tile.Split(',');
            string id = pieces[0];

            string[] parsedCoords = pieces[1].Split('_');
            int[] intCoords = Array.ConvertAll<string,int>(parsedCoords, int.Parse);
            Vector3 scaledCoords = new Vector3(intCoords[0]*(tileSize+tileGap), 0, intCoords[1]*(tileSize + tileGap));

            string type = pieces[2];

            string[] nextTiles = pieces[3].Split('_');
            string[] prevTiles = pieces[4].Split('_');

            Tile newTile = new Tile(id, scaledCoords, type, nextTiles, prevTiles);

            tileData.Add(id, newTile);
            if (type == "home") {
                homeIDs.Add(id);
            }

            GameObject tempTile = Instantiate(tilePrefab, scaledCoords, Quaternion.identity);
            MeshRenderer renderer = tempTile.GetComponentInChildren<MeshRenderer>();

            /* Color Assignment
             * Warp = purple
             * Encounter = red
             * Drop = blue
             * Draw = green
             * bonus = gold
             * home = white
             */
            switch (type) {
                case "warp":
                    renderer.material.color = Color.purple;
                    break;
                case "encounter":
                    renderer.material.color = Color.red;
                    break;
                case "drop":
                    renderer.material.color = Color.blue;
                    break;
                case "draw":
                    renderer.material.color = Color.green;
                    break;
                case "bonus":
                    renderer.material.color = Color.gold;
                    break;
                case "home":
                    renderer.material.color = Color.white;
                    break;
            }
        }
    }
}
