using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    [SerializeField] private TilePool TilePool;

    public Action<int> OnGridAdvanced;
    
    public readonly Queue<List<Tile>> ActiveRows = new(InitGridLength);
    
    private const float GridAdvanceTimeInterval = 0.35f;

    private const int InitGridWidth = 24; // NOTE: affects MinIrregularTilesPerRow
    private const int InitGridLength = 70;
    private const float MinIrregularTilesPerRow = 5;  // NOTE: affects InitGridWidth
    
    private const float BorderVariationProbability = 0.1f;
    private readonly (TileType, float)[] _tileProbabilities = 
    {
        (TileType.Hole, 0.1f),
        (TileType.Obstacle, 0.2f),
        (TileType.Regular, 0.7f)
    };
    
    public void StartGeneration()
    {
        for (var i = 0; i < InitGridLength; i++)
        {
            var row = SpawnRow(i);
            ActiveRows.Enqueue(row);
        }
        
        StartCoroutine(AdvanceTileGrid());
    }

    public int CurrentRowIndex = InitGridLength;
    private IEnumerator AdvanceTileGrid()
    {
        for (var i = CurrentRowIndex; ; i++)
        {
            RemoveLastRow();
            var row = SpawnRow(i);
            ActiveRows.Enqueue(row);
            OnGridAdvanced?.Invoke(i);
            yield return new WaitForSeconds(GridAdvanceTimeInterval);
        }
    }

    private List<Tile> SpawnRow(int index)
    {
        var row = new List<Tile>(InitGridWidth);
        var irregularTilesLeft = InitGridWidth - MinIrregularTilesPerRow;
        for (var i = 0; i < InitGridWidth; i++)
        {
            TileType tileType;
            if (i is 0 or InitGridWidth - 1)
            {
                tileType = TileType.Border;
            }
            else if (i is 1 or InitGridWidth - 2)
            {
                tileType = Random.value <= BorderVariationProbability
                    ? TileType.Border
                    : GetRouletteTileType(_tileProbabilities);
            }
            else
            {
                if (irregularTilesLeft == 0)
                {
                    tileType = TileType.Regular;
                }
                else
                {
                    tileType = GetRouletteTileType(_tileProbabilities);
                    if (tileType != TileType.Regular)
                    {
                        irregularTilesLeft--;
                    }
                }
            }

            var tile = TilePool.GetOrInstantiateTile(tileType);
            var x = index / 2 + InitGridWidth - i;
            var z = index / 2 + index % 2 + i + 1;
            tile.GameObject.transform.position = transform.position + new Vector3(x, 0, z);
            row.Add(tile);
        }

        return row;
    }
    
    private void RemoveLastRow()
    {
        foreach (var tile in ActiveRows.Dequeue())
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
