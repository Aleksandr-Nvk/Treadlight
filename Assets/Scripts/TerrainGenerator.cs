using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    private TilePool TilePool;
    
    [SerializeField]
    private Cube Cube;
    
    public const int InitGridWidth = 24;
    public const int InitGridLength = 70;
    
    private const float BorderVariationProbability = 0.1f;
    private readonly (TileType, float)[] _tileProbabilities = 
    {
        (TileType.Hole, 0.05f),
        (TileType.Obstacle, 0.05f),
        (TileType.Regular, 0.9f)
    };
    
    private readonly Queue<List<Tile>> _rows = new(InitGridLength);

    public void Awake()
    {
        Cube.OnCubeMoved += moveType =>
        {
            if (moveType is Cube.MoveType.BottomLeft or Cube.MoveType.BottomRight)
            {
                //RemoveLastRow();
            }
        };
    }

    public void Start()
    {
        for (var i = 0; i < InitGridLength; i++)
        {
            var row = SpawnRow(i / 2, i / 2 + i % 2);
            _rows.Enqueue(row);
        }

        StartCoroutine(AdvanceTileGrid());
    }

    private IEnumerator AdvanceTileGrid()
    {
        for (var i = 0;; i++)
        {
            RemoveLastRow();
            var row = SpawnRow((InitGridLength + i) / 2, (InitGridLength + i) / 2 + (InitGridLength + i) % 2);
            _rows.Enqueue(row);
            yield return new WaitForSeconds(0.25f);
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

            var tile = TilePool.GetOrInstantiateTile(tileType);
            tile.GameObject.transform.position = transform.position + new Vector3(x + InitGridWidth - i, 0, z + i + 1);
            row.Add(tile);
        }

        return row;
    }
    
    private void RemoveLastRow()
    {
        foreach (var tile in _rows.Dequeue())
        {
            TilePool.RemoveTile(tile);
        }
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
