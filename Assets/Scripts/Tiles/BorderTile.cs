using UnityEngine;

namespace Tiles
{
    public class BorderTile : MonoBehaviour
    {
        private const float AnomalyProbability = 0.025f;
        private void Start()
        {
            var tilePaletteManager = TilePaletteManager.GetInstance();
            TilePaletteManager.ChangeMaterialColor(gameObject, tilePaletteManager.GetPalette()[0]);
            
            var innerTile = transform.GetChild(0);
            innerTile.localScale = Random.value < AnomalyProbability
                ? new Vector3(Random.Range(0.95f, 1f), Random.Range(2f, 8f), Random.Range(0.95f, 1f))
                : new Vector3(Random.Range(0.85f, 1.15f), Random.Range(0.85f, 1.15f), Random.Range(0.85f, 1.15f));
            
            innerTile.localPosition = new Vector3(Random.Range(0f, 0.25f), Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
            transform.localScale += Vector3.up * Random.Range(0f, 3f);
        }
    }
}
