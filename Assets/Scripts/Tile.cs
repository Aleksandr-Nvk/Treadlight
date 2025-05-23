using UnityEngine;

public struct Tile
{
    public GameObject GameObject;
    public TileType Type;

    public Tile(GameObject gameObject, TileType type)
    {
        GameObject = gameObject;
        Type = type;
    }
}
    
public enum TileType
{
    Regular,
    Hole,
    Border,
    Obstacle
}