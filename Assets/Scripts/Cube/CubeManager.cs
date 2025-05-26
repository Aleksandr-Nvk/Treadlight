using System;
using System.Linq;
using DG.Tweening;
using Tiles;
using UnityEngine;

namespace Cube
{
    public class CubeManager : MonoBehaviour
    {
        [SerializeField] private Cube Cube;
    
        [SerializeField] private InputManager InputManager;
        [SerializeField] private TileManager TileManager;
    
        [NonSerialized] public Action OnCubeDestroyed;
    
        private Vector3 _initCubeParentPosition;
        private Vector3 _initCubePosition;

        private const float CubeSpawnHeight = 50f;

        private void Awake()
        {
            Cube.OnMoved += HandleCubeMove;
            TileManager.OnGridAdvanced += HandleCubePosition;
        
            InputManager.OnTopRightSwipe += () => Cube.Move(CubeMoveType.TopRight);
            InputManager.OnTopLeftSwipe += () => Cube.Move(CubeMoveType.TopLeft);
            InputManager.OnBottomRightSwipe += () => Cube.Move(CubeMoveType.BottomRight);
            InputManager.OnBottomLeftSwipe += () => Cube.Move(CubeMoveType.BottomLeft);
        }

        private void Start()
        {
            _initCubeParentPosition = Cube.transform.parent.position;
            _initCubePosition = Cube.transform.position;
        }

        // cube's Transform IS reset do defaults
        public void SpawnCube()
        {
            Cube.gameObject.SetActive(true);
            Cube.transform.parent.position = _initCubeParentPosition;
            Cube.transform.position = _initCubePosition + Vector3.up * CubeSpawnHeight;
            Cube.transform.rotation = Quaternion.identity;
            Cube.transform.localScale = new Vector3(1.2f, 1f, 1.2f);
        
            var animationSequence = DOTween.Sequence();
            animationSequence.AppendInterval(0.5f);
            animationSequence.Append(Cube.transform.DOLocalMove(_initCubePosition, 0.25f));
            animationSequence.Append(Cube.transform.DOScale(Vector3.one, 0.1f));
            animationSequence.OnComplete(() => Cube.IsMovementBlocked = false);
        }
    
        // cube's Transform IS NOT reset to defaults
        public void DestroyCube(bool silently = false)
        {
            Cube.IsMovementBlocked = true;
            if (silently)
            {
                Cube.transform.localScale = Vector3.zero;
                Cube.gameObject.SetActive(false);
            }
            else
            {
                var seq = DOTween.Sequence();
                seq.Append(Cube.transform.DOScale(Vector3.zero, 0.25f));
                seq.OnComplete(() =>
                {
                    Cube.gameObject.SetActive(false);
                    OnCubeDestroyed?.Invoke();
                });
            }
        }
        
        private void HandleCubeMove(TileType targetTileType)
        {
            if (targetTileType == TileType.Hole)
            {
                DestroyCube();
            }
        }

        private void HandleCubePosition(int gridRowIndex)
        {
            if (!Cube.gameObject.activeSelf) return;
            
            var isCubeInVoid = TileManager.ActiveRows.First().All(tile =>
            {
                var tilePosition = tile.GameObject.transform.position;
                var cubePosition = Cube.transform.position;
                return tilePosition.x > cubePosition.x || tilePosition.z > cubePosition.z;
            });
            
            if (isCubeInVoid)
            {
                DestroyCube();
            }
        }
    }
}