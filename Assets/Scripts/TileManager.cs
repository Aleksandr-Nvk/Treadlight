using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    [SerializeField] private TilePool TilePool;
    [SerializeField] private Cube Cube;

    public Action<int> OnGridAdvanced;
    
    public readonly Queue<List<Tile>> ActiveRows = new(InitGridLength);

    private readonly Vector3 _gridSpawnOffset = new(-30, -1, -30);
    
    private const float GridAdvanceTimeInterval = 0.35f;

    private const int InitGridWidth = 25;
    private const int InitGridLength = 70;
    private const int MaxIrregularTilesPerRow = InitGridWidth / 2 + 1;
    
    private const float BorderVariationProbability = 0.1f;
    private readonly (TileType, float)[] _tileProbabilities = 
    {
        (TileType.Hole, 0.1f),
        (TileType.Obstacle, 0.2f),
        (TileType.Regular, 0.7f)
    };
    
    public void GenerateStartGrid()
    {
        for (var i = 0; i < InitGridLength; i++)
        {
            var offsetCubePosition = Cube.transform.parent.position - _gridSpawnOffset;
            var positionSum = Mathf.Abs(offsetCubePosition.x + offsetCubePosition.z);
            
            // protection against unfortunate spawn
            // ridiculously complicated formula calculated on a sheet of paper. Do NOT touch or face the consequences
            if (Mathf.Approximately(positionSum, InitGridWidth + 2f * Mathf.Ceil((i - 1) / 2f) + 1f))
            {
                ActiveRows.Enqueue(SpawnRow(i++, true));
                ActiveRows.Enqueue(SpawnRow(i, true));
            }
            else
            {
                ActiveRows.Enqueue(SpawnRow(i));
            }
        }

        _currentIndex = InitGridLength;
    }

    private bool _isGenerationAllowed = false;

    public void StartGeneration() => _isGenerationAllowed = true;
    public void StopGeneration() => _isGenerationAllowed = false;

    private float _timePassed = 0f;
    private int _currentIndex = 0;
    private void Update()
    {
        if (!_isGenerationAllowed) return;
        
        _timePassed += Time.deltaTime;
        if (!(_timePassed >= GridAdvanceTimeInterval)) return;
        
        RemoveLastRow();
        ActiveRows.Enqueue(SpawnRow(_currentIndex));
        OnGridAdvanced?.Invoke(_currentIndex);

        _currentIndex++;
        _timePassed = 0f;
    }

    private List<Tile> SpawnRow(int index, bool isFullRegularRow = false)
    {
        var row = new List<Tile>(InitGridWidth);
        var irregularTilesLeft = InitGridWidth - MaxIrregularTilesPerRow;
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
                if (!isFullRegularRow)
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
                else
                {
                    tileType = TileType.Regular;
                }
            }

            var tile = TilePool.GetOrInstantiateTile(tileType);
            var tilePosX = index / 2 + InitGridWidth - i;
            var tilePosZ = index / 2 + index % 2 + i + 1;
            tile.GameObject.transform.position = new Vector3(tilePosX, 0, tilePosZ) + _gridSpawnOffset;
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
    
    public void ClearGrid()
    {
        while (ActiveRows.Count != 0)
        {
            RemoveLastRow();
        }

        _currentIndex = 0;
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
