using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private InputManager InputManager;
    [SerializeField] private TileManager TileManager;
    
    [NonSerialized] public bool IsMovementBlocked = true;

    [NonSerialized] public Action<TileType> OnMoved;
    
    private const float AnimationDuration = 0.25f;
    
    private void Awake()
    {
        InputManager.OnTopRightSwipe += () => Move(MoveType.TopRight);
        InputManager.OnTopLeftSwipe += () => Move(MoveType.TopLeft);
        InputManager.OnBottomRightSwipe += () => Move(MoveType.BottomRight);
        InputManager.OnBottomLeftSwipe += () => Move(MoveType.BottomLeft);
    }

    private void Move(MoveType moveType)
    {
        if (IsMovementBlocked) return;
        
        var endPosition = transform.parent.position;
        var endRotation = transform.rotation.eulerAngles;
        switch (moveType)
        {
            case MoveType.TopRight:
                endPosition.x -= 1;
                endRotation.z += 90;
                break;
            case MoveType.TopLeft:
                endPosition.z -= 1;
                endRotation.x -= 90;
                break;
            case MoveType.BottomRight:
                endPosition.z += 1;
                endRotation.x += 90;
                break;
            case MoveType.BottomLeft:
                endPosition.x += 1;
                endRotation.z -= 90;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
        }

        var tileRows = TileManager.ActiveRows.SelectMany(row => row);
        Tile targetTile;
        try
        {
            targetTile = tileRows.First(tile => tile.GameObject.transform.position == endPosition + Vector3.down);
        }
        catch (InvalidOperationException) // tried to jump into the void
        {
            OnMoved(TileType.Hole);
            return;
        }
        
        switch (targetTile.Type)
        {
            case TileType.Regular:
            case TileType.Hole:
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
    
    private enum MoveType
    {
        TopRight,
        TopLeft,
        BottomRight,
        BottomLeft
    }
}
