using Cube;
using DG.Tweening;
using Tiles;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private CubeManager CubeManager;
    
    private const float AnimationDuration = 0.5f;

    private Camera _camera;
    
    private Vector3 _initPosition;
    private Vector3 _positionDelta;

    private bool _isIdle = true;

    private void Awake()
    {
        CubeManager.OnCubeDestroyed += ZoomOut;
    }

    private void Start()
    {
        var tilePaletteManager = TilePaletteManager.GetInstance();
        _camera = GetComponent<Camera>();
        _camera.backgroundColor = tilePaletteManager.GetPalette()[3];
        _positionDelta = transform.position - Target.position;
        _initPosition = transform.position;
    }

    public void EnableCameraIdle()
    {
        _isIdle = true;
        transform.DOMove(_initPosition, AnimationDuration);
        ZoomOut();
    }

    public void DisableCameraIdle()
    {
        _isIdle = false;
        transform.DOMove(_initPosition, AnimationDuration * 2f);
        ZoomIn();
    }

    private void ZoomIn() => _camera.DOOrthoSize(10, AnimationDuration);

    private void ZoomOut() => _camera.DOOrthoSize(12, AnimationDuration);

    private void Update()
    {
        if (_isIdle)
        {
            transform.DOMove(transform.position + new Vector3(1f, 0f, 1f) * 0.65f, 1f).SetEase(Ease.Linear);
        }
        else
        {
            transform.DOMove(Target.position + _positionDelta, AnimationDuration);
        }
    }
}
