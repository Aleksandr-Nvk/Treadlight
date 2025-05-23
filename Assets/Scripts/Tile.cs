using UnityEngine;

public struct Tile
{
    public readonly GameObject GameObject;
    public readonly TileType Type;

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