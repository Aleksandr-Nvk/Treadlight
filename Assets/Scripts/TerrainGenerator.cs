using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject GroundTile;
    
    private const int GridWidth = 40;
    private const int GridLength = 60;

    private const float HoleProbability = 0.05f;

    public void Start()
    {
        for (var x = 0; x < GridLength; x++)
        {
            for (var z = x; z < GridWidth + x; z++)
            {
                if (Random.value < HoleProbability) continue;
                Instantiate(GroundTile, transform.position + new Vector3(x, 0, z), Quaternion.identity, transform);
            }
        }
    }
}
