using Cube;
using DG.Tweening;
using Tiles;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform Cube;
    [SerializeField] private CubeManager CubeManager;
    private Transform _target;

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
        _target = Cube.parent;
        var tilePaletteManager = TilePaletteManager.GetInstance();
        _camera = GetComponent<Camera>();
        _camera.backgroundColor = tilePaletteManager.GetPalette()[3];
        _positionDelta = transform.position - _target.position;
        _initPosition = transform.position;
    }

    public void EnableCameraIdle()
    {
        transform.DOMove(_initPosition, AnimationDuration).OnComplete(() => _isIdle = true);
        ZoomOut();
    }

    public void DisableCameraIdle()
    {
        _isIdle = false;
        transform.DOMove(_initPosition, AnimationDuration * 2f);
        ZoomIn();
    }

    public void ZoomIn() => _camera.DOOrthoSize(10, AnimationDuration).SetEase(Ease.OutExpo);

    public void ZoomOut() => _camera.DOOrthoSize(12, AnimationDuration).SetEase(Ease.OutExpo);

    private void Update()
    {
        if (_isIdle)
        {
            transform.DOMove(transform.position + new Vector3(1f, 0f, 1f) * 0.65f, 1f).SetEase(Ease.Linear);
        }
        else if (Cube.gameObject.activeSelf)
        {
            transform.DOMove(_target.position + _positionDelta, AnimationDuration);
        }
    }
}
