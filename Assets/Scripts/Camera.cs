using DG.Tweening;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private Transform Target;

    private const float FollowDuration = 0.5f;
    
    private Vector3 _positionDelta;
    
    private void Start()
    {
        _positionDelta = transform.position - Target.position;
    }
    
    private void Update()
    {
        transform.DOMove(Target.position + _positionDelta, FollowDuration);
    }
}
