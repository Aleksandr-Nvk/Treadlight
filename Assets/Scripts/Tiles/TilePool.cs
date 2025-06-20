﻿using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tiles
{
    public class TilePool : MonoBehaviour
    {
        [SerializeField] private GameObject RegularGroundTile;
        [SerializeField] private GameObject BorderGroundTile;
        [SerializeField] private GameObject ObstacleGroundTile;
        [SerializeField] private GameObject HoleGroundTile;
        [SerializeField] private GameObject StainGroundTile;

        private const float TileAnomalyProbability = 0.025f;
        
        private readonly Dictionary<TileType, Queue<Tile>> _pool = new();

        private void Awake()
        {
            _pool[TileType.Regular] = new Queue<Tile>();
            _pool[TileType.Border] = new Queue<Tile>();
            _pool[TileType.Obstacle] = new Queue<Tile>();
            _pool[TileType.Hole] = new Queue<Tile>();
            _pool[TileType.Stain] = new Queue<Tile>();
        }

        public Tile GetOrInstantiateTile(TileType type)
        {
            var tilePrefab = type switch
            {
                TileType.Regular => RegularGroundTile,
                TileType.Border => BorderGroundTile,
                TileType.Obstacle => ObstacleGroundTile,
                TileType.Hole => HoleGroundTile,
                TileType.Stain => StainGroundTile,
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
                tileObject = InstantiateTile(type, tilePrefab);
            }

            var tileTransform = tileObject.transform;
            tileTransform.localScale = Vector3.zero;
            tileTransform.DOScale(1f, 0.5f * Random.Range(0.5f, 1.5f))
                         .SetEase(Ease.InOutBounce);
            
            return new Tile(tileObject, type);
        }

        private GameObject InstantiateTile(TileType type, GameObject prefab)
        {
            if (type is TileType.Hole or TileType.Stain) return Instantiate(prefab, Vector3.zero, Quaternion.identity);

            var tilePaletteManager = TilePaletteManager.GetInstance();
            var tileObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            int colorIndex;

            switch (type)
            {
                case TileType.Regular:
                    colorIndex = 0;
                    for (var i = 0; i < tileObject.transform.childCount; i++)
                    {
                        var innerRegularTile = tileObject.transform.GetChild(i);
                        if (Random.value > 0.25f)
                        {
                            innerRegularTile.gameObject.SetActive(false);
                        }
                        else
                        {
                            innerRegularTile.localEulerAngles = new Vector3(90f * Random.Range(1, 3), 0f, 90f * Random.Range(1, 3));
                            innerRegularTile.localPosition += new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
                        }
                    }
                    break;
                case TileType.Border:
                    colorIndex = 1;
                    var innerBorderTile = tileObject.transform.GetChild(0);
                    innerBorderTile.localScale = Random.value < TileAnomalyProbability
                        ? new Vector3(Random.Range(0.95f, 1f), Random.Range(5f, 10f), Random.Range(0.95f, 1f))
                        : new Vector3(Random.Range(0.85f, 1.15f), Random.Range(0.5f, 6f), Random.Range(0.85f, 1.15f));
            
                    innerBorderTile.localPosition = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
                    tileObject.transform.localScale += Vector3.up * Random.Range(0f, 3f);
                    break;
                case TileType.Obstacle:
                    colorIndex = 2;
                    var innerObstacleTile = tileObject.transform.GetChild(0);
                    innerObstacleTile.localScale = Random.value < TileAnomalyProbability
                        ? new Vector3(Random.Range(0.95f, 1f), Random.Range(2f, 8f), Random.Range(0.95f, 1f))
                        : new Vector3(Random.Range(0.65f, 0.9f), innerObstacleTile.localScale.y, Random.Range(0.65f, 0.9f));
            
                    innerObstacleTile.localPosition += Vector3.up * Random.Range(-0.25f, 0.5f);
                    break;
                case TileType.Stain:
                case TileType.Hole:
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
           
            TilePaletteManager.ChangeMaterialColor(tileObject, tilePaletteManager.GetPalette()[colorIndex]);
            return tileObject;
        }

        public void RemoveTile(Tile tile)
        {
            var tileTransform = tile.GameObject.transform;
            tileTransform.DOScale(0f, 0.5f * Random.Range(0.5f, 1.5f))
                         .SetEase(Ease.InOutBounce)
                         .OnComplete(() =>
                         {
                             tileTransform.gameObject.SetActive(false);
                             tileTransform.localScale = Vector3.one;
                             _pool[tile.Type].Enqueue(tile);
                         });
        }
    }
}