using System;
using System.Linq;
using DG.Tweening;
using Tiles;
using UnityEngine;

namespace Cube
{
    public class Cube : MonoBehaviour
    {
        [SerializeField] private TileManager TileManager;
    
        [NonSerialized] public bool IsMovementBlocked = true;
        
        [NonSerialized] public Action<TileType> OnMoved;
    
        private const float AnimationDuration = 0.25f;
    
        public void Move(CubeMoveType cubeMoveType)
        {
            if (IsMovementBlocked) return;
        
            var endPosition = transform.parent.position;
            var endRotation = transform.rotation.eulerAngles;
            switch (cubeMoveType)
            {
                case CubeMoveType.TopRight:
                    endPosition.x -= 1;
                    endRotation.z += 90;
                    break;
                case CubeMoveType.TopLeft:
                    endPosition.z -= 1;
                    endRotation.x -= 90;
                    break;
                case CubeMoveType.BottomRight:
                    endPosition.z += 1;
                    endRotation.x += 90;
                    break;
                case CubeMoveType.BottomLeft:
                    endPosition.x += 1;
                    endRotation.z -= 90;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cubeMoveType), cubeMoveType, null);
            }

            var tileRows = TileManager.ActiveRows.SelectMany(row => row);
            Tile targetTile;
            try
            {
                targetTile = tileRows.First(tile => tile.GameObject.transform.position == endPosition + Vector3.down);
            }
            catch (InvalidOperationException) // tried to jump into the void
            {
                OnMoved?.Invoke(TileType.Hole);
                return;
            }
        
            switch (targetTile.Type)
            {
                case TileType.Regular:
                case TileType.Hole:
                case TileType.Stain:
                    break;
                case TileType.Border:
                case TileType.Obstacle:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        
            IsMovementBlocked = true;
        
            var seq = DOTween.Sequence();
            seq.Append(transform.parent.DOMove(endPosition, AnimationDuration));
            seq.Join(transform.DOLocalRotate(endRotation, AnimationDuration));
            seq.OnComplete(() => 
            {
                transform.rotation = Quaternion.identity;
                IsMovementBlocked = false;
                OnMoved?.Invoke(targetTile.Type);
            });
        }
    
    
    }
}
