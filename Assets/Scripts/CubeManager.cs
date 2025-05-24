using DG.Tweening;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private Cube Cube;
    
    private const float CubeSpawnHeight = 50f;
    
    public void SpawnCube()
    {
        Cube.gameObject.SetActive(true);
        var initCubePosition = Cube.transform.position;
        Cube.transform.position += Vector3.up * CubeSpawnHeight;
        Cube.transform.localScale = new Vector3(1.2f, 1f, 1.2f);
        
        var animationSequence = DOTween.Sequence();
        animationSequence.AppendInterval(0.5f);
        animationSequence.Append(Cube.transform.DOMove(initCubePosition, 0.25f));
        animationSequence.Append(Cube.transform.DOScale(Vector3.one, 0.1f));
        animationSequence.OnComplete(() => Cube.IsMovementBlocked = false);
    }
}