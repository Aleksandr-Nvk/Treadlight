using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private Cube Cube;
    
    [SerializeField] private TileManager TileManager;
    
    [NonSerialized] public Action OnCubeDestroyed;
    
    private Vector3 _initCubePosition;

    private const float CubeSpawnHeight = 50f;

    private void Awake()
    {
        Cube.OnMoved += HandleCubeMove;
        TileManager.OnGridAdvanced += HandleCubePosition;
    }

    private void Start()
    {
        _initCubePosition = Cube.transform.position;
    }

    private void HandleCubeMove(TileType targetTileType)
    {
        if (targetTileType == TileType.Hole)
        {
            Debug.Log("Dead. Reason: hole");
            DestroyCube();
        }
    }

    private void HandleCubePosition(int gridRowIndex)
    {
        if (TileManager.ActiveRows.First().All(tile =>
            {
                var tilePosition = tile.GameObject.transform.position;
                var cubePosition = Cube.transform.position;
                return tilePosition.x > cubePosition.x || tilePosition.z > cubePosition.z;
            }))
        {
            Debug.Log("Dead. Reason: outside of grid");
            DestroyCube();
        }
    }

    // cube's Transform is reset do defaults
    public void SpawnCube()
    {
        Cube.gameObject.SetActive(true);
        Cube.transform.position = _initCubePosition + Vector3.up * CubeSpawnHeight;
        Cube.transform.rotation = Quaternion.identity;
        Cube.transform.localScale = new Vector3(1.2f, 1f, 1.2f);
        
        var animationSequence = DOTween.Sequence();
        animationSequence.AppendInterval(0.5f);
        animationSequence.Append(Cube.transform.DOMove(_initCubePosition, 0.25f));
        animationSequence.Append(Cube.transform.DOScale(Vector3.one, 0.1f));
        animationSequence.OnComplete(() => Cube.IsMovementBlocked = false);
    }
    
    // cube's Transform is not reset to defaults
    private void DestroyCube()
    {
        Cube.IsMovementBlocked = true;
        var seq = DOTween.Sequence();
        seq.Append(Cube.transform.DOScale(Vector3.zero, 0.25f));
        seq.Join(Cube.transform.DOMove(Cube.transform.position + Vector3.down, 0.25f));
        seq.OnComplete(() =>
        {
            Cube.gameObject.SetActive(false);
            OnCubeDestroyed?.Invoke();
        }); 
    }
}