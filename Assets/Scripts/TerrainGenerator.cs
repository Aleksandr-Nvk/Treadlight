using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject RegularGroundTile;
    [SerializeField]
    private GameObject BorderGroundTile;
    [SerializeField]
    private GameObject ObstacleGroundTile;
    
    [SerializeField]
    private Transform Cube;

    private readonly (TileType, float)[] _tileProbabilities = 
    {
        (TileType.Hole, 0.05f),
        (TileType.Obstacle, 0.05f),
        (TileType.Regular, 0.9f)
    };

    private const float BorderVariationProbability = 0.1f;
    
    private const int InitGridWidth = 24;
    private const int InitGridLength = 70;
    
    private readonly List<List<Tile>> _rows = new(InitGridLength);

    public void Start()
    {
        for (var i = 0; i < InitGridLength; i++)
        {
            var row = SpawnRow(i / 2, i / 2 + i % 2);
            _rows.Add(row);
        }
    }

    private List<Tile> SpawnRow(int x, int z)
    {
        var row = new List<Tile>(InitGridWidth);
        for (var i = 0; i < InitGridWidth; i++)
        {
            var tileType = i switch
            {
                0 or InitGridWidth - 1 => TileType.Border,
                1 or InitGridWidth - 2 => Random.value <= BorderVariationProbability
                    ? TileType.Border
                    : GetRouletteTileType(_tileProbabilities),
                _ => GetRouletteTileType(_tileProbabilities)
            };

            var tile = SpawnTile(x + InitGridWidth - i, z + i + 1, tileType);
            row.Add(tile);
        }

        return row;
    }
    
    private Tile SpawnTile(int x, int z, TileType type)
    {
        GameObject tilePrefab;
        switch (type)
        {
            case TileType.Regular:
                tilePrefab = RegularGroundTile;
                break;
            case TileType.Border:
                tilePrefab = BorderGroundTile;
                break;
            case TileType.Obstacle:
                tilePrefab = ObstacleGroundTile;
                break;
            case TileType.Hole:
                return new Tile(null, type);
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
        var tileObject = Instantiate(tilePrefab, transform.position + new Vector3(x, 0, z), Quaternion.identity, transform);
        return new Tile(tileObject, type);
    }
    
    // shamelessly stolen from https://gamedev.stackexchange.com/a/153851
    private static TileType GetRouletteTileType((TileType, float)[] weights)
    {
        var total = weights.Select(weight => weight.Item2).Sum();
        if (total <= 0f) throw new ArgumentException("Total weight must be positive.");

        var amount = (float)(new System.Random().NextDouble() * total);
        var cumulative = 0f;
        for (var i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i].Item2;
            if (amount <= cumulative) return weights[i].Item1;
        }

        return weights.Last().Item1;
    }
}
