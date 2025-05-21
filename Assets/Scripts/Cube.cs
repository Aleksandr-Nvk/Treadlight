using DG.Tweening;
using UnityEngine;

public class Cube : MonoBehaviour
{
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

    private void Move(MoveType moveType)
    {
        var endPosition = transform.position;
        switch (moveType)
        {
            case MoveType.TopRight:
                endPosition.x -= 1;
                break;
            case MoveType.TopLeft:
                endPosition.z -= 1;
                break;
            case MoveType.BottomRight:
                endPosition.z += 1;
                break;
            case MoveType.BottomLeft:
                endPosition.x += 1;
                break;
        }

        transform.DOMove(endPosition, 0.5f, true);
    }
    
    private enum MoveType
    {
        TopRight,
        TopLeft,
        BottomRight,
        BottomLeft
    }
}
