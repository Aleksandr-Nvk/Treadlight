using DG.Tweening;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private const float AnimationDuration = 0.25f;

    private CubeInputManager _inputManager;

    public void Init(CubeInputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Awake()
    {
        _inputManager.OnTopRightSwipe += () => Move(MoveType.TopRight);
        _inputManager.OnTopLeftSwipe += () => Move(MoveType.TopLeft);
        _inputManager.OnBottomRightSwipe += () => Move(MoveType.BottomRight);
        _inputManager.OnBottomLeftSwipe += () => Move(MoveType.BottomLeft);
    }

    private bool _isMovementBlocked = false;
        
    private void Move(MoveType moveType)
    {
        if (_isMovementBlocked) return;
        
        var endPosition = transform.parent.position;
        var endRotation = transform.rotation.eulerAngles; // bottom-left, top-left, bottom-left
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
