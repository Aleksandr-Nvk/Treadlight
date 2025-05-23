using System;
using System.Collections.Generic;
using UnityEngine;

public class TilePool : MonoBehaviour
{
    [SerializeField]
    private  GameObject RegularGroundTile;
    [SerializeField]
    private GameObject BorderGroundTile;
    [SerializeField]
    private GameObject ObstacleGroundTile;
    [SerializeField]
    private GameObject HoleGroundTile;

    private readonly Dictionary<TileType, Queue<Tile>> _pool = new();

    private void Awake()
    {
        _pool[TileType.Regular] = new Queue<Tile>();
        _pool[TileType.Border] = new Queue<Tile>();
        _pool[TileType.Obstacle] = new Queue<Tile>();
        _pool[TileType.Hole] = new Queue<Tile>();
    }

    public Tile GetOrInstantiateTile(TileType type)
    {
        var tilePrefab = type switch
        {
            TileType.Regular => RegularGroundTile,
            TileType.Border => BorderGroundTile,
            TileType.Obstacle => ObstacleGroundTile,
            TileType.Hole => HoleGroundTile,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        GameObject tileObject;
        if (_pool[type].Count != 0)
        {
            tileObject = _pool[type].Dequeue().GameObject;
            tileObject.SetActive(true);
        }
        else
        {
            tileObject = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
        }

        return new Tile(tileObject, type);
    }

    public void RemoveTile(Tile tile)
    {
        tile.GameObject.SetActive(false);
        _pool[tile.Type].Enqueue(tile);
    }
}