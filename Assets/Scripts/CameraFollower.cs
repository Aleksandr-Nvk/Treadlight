using DG.Tweening;
using Tiles;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform Target;

    private const float FollowDuration = 0.5f;
    
    private Vector3 _positionDelta;

    private bool _isIdle = true;

    private Vector3 _initPosition;
    
    private void Start()
    {
        var tilePaletteManager = TilePaletteManager.GetInstance();
        GetComponent<Camera>().backgroundColor = tilePaletteManager.GetPalette()[3];
        _positionDelta = transform.position - Target.position;
        _initPosition = transform.position;
    }

    public void EnableCameraIdle()
    {
        _isIdle = true;
        transform.DOMove(_initPosition, FollowDuration);
    }

    public void DisableCameraIdle()
    {
        _isIdle = false;
        transform.DOMove(_initPosition, FollowDuration);
    }

    private void Update()
    {
        if (_isIdle)
        {
            transform.position += new Vector3(1, 0, 1) * (Mathf.Sqrt(2) * 0.875f * Time.deltaTime);
        }
        else
        {
            transform.DOMove(Target.position + _positionDelta, FollowDuration);
        }
    }
}
