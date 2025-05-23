using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private InputManager InputManager;
    
    [SerializeField]
    private TerrainGenerator TerrainGenerator;
    
    private const float AnimationDuration = 0.25f;
    
    private void Awake()
    {
        InputManager.OnTopRightSwipe += () => Move(MoveType.TopRight);
        InputManager.OnTopLeftSwipe += () => Move(MoveType.TopLeft);
        InputManager.OnBottomRightSwipe += () => Move(MoveType.BottomRight);
        InputManager.OnBottomLeftSwipe += () => Move(MoveType.BottomLeft);
    }

    private bool _isMovementBlocked = false;
    private void Move(MoveType moveType)
    {
        if (_isMovementBlocked) return;
        
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

        var targetTile = TerrainGenerator.ActiveRows
                                         .SelectMany(row => row)
                                         .First(tile => tile.GameObject.transform.position == endPosition + Vector3.down);
        switch (targetTile.Type)
        {
            case TileType.Regular:
                Debug.Log("Safe");
                break;
            case TileType.Hole:
                Debug.Log("Dead");
                return;
            case TileType.Border:
            case TileType.Obstacle:
                Debug.Log("Unavailable");
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _isMovementBlocked = true;
        transform.parent.DOMove(endPosition, AnimationDuration);
        transform.DOLocalRotate(endRotation, AnimationDuration) // TODO: rotate around ribs, not center
                 .OnComplete(() =>
                 {
                     transform.rotation = Quaternion.identity;
                     _isMovementBlocked = false;
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
