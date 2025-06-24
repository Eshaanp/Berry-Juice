using UnityEngine;

public class Tile 
    //Stores relevant information about individual tiles
{
    private string id;
    private Vector3 coordinate; //(x, y, z)
    private string type; //warp, gain, loss, neutral, etc
    private string[] nextTiles;
    private string[] prevTiles;

    public Tile (string id, Vector3 coordinate, string type, string[] nextTiles, string[] prevTiles) {
        this.id = id;
        this.coordinate = coordinate;
        this.type = type;
        this.nextTiles = nextTiles;
        this.prevTiles = prevTiles;
    }

    public void setCoord(Vector3 coord) {
        this.coordinate = coord;
    }

    public Vector3 getCoordinate() {
        return this.coordinate;
    }
}
