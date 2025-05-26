using UnityEngine;

namespace Tiles
{
    public class ObstacleTile : MonoBehaviour
    {
        private const float AnomalyProbability = 0.025f;
        private void Start()
        {
            var tilePaletteManager = TilePaletteManager.GetInstance();
            TilePaletteManager.ChangeMaterialColor(gameObject, tilePaletteManager.GetPalette()[1]);
           
            var innerTile = transform.GetChild(0);
            innerTile.localScale = Random.value < AnomalyProbability
                ? new Vector3(Random.Range(0.95f, 1f), Random.Range(2f, 8f), Random.Range(0.95f, 1f))
                : new Vector3(Random.Range(0.65f, 0.9f), innerTile.localScale.y, Random.Range(0.65f, 0.9f));
            
            innerTile.localPosition += Vector3.up * Random.Range(-0.25f, 0.5f);
        }
    }
}
