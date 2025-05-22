using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject GroundTile;
    
    private const int InitGridWidth = 24;
    private const int InitGridLength = 70;

    private const float HoleProbability = 0.05f;

    public void Start()
    {
        for (var i = 0; i < InitGridLength; i++)
        {
            SpawnRow(i / 2,  i / 2 + i % 2);
        }
    }

    private void SpawnRow(int x, int z)
    {
        for (var i = 0; i < InitGridWidth; i++)
        {
            SpawnTile(x + InitGridWidth - i, z + i + 1);
        }
    }
    
    private void SpawnTile(int x, int z)
    {
        if (Random.value < HoleProbability) return;
        Instantiate(GroundTile, transform.position + new Vector3(x, 0, z), Quaternion.identity, transform);
    }
}
