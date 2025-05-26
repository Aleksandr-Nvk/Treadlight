using DG.Tweening;
using Tiles;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform Target;

    private const float FollowDuration = 0.5f;
    
    private Vector3 _positionDelta;
    
    private void Start()
    {
        var tilePaletteManager = TilePaletteManager.GetInstance();
        GetComponent<Camera>().backgroundColor = tilePaletteManager.GetPalette()[3];
        _positionDelta = transform.position - Target.position;
    }
    
    private void Update()
    {
        transform.DOMove(Target.position + _positionDelta, FollowDuration);
    }
}
