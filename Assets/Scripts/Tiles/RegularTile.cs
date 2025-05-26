using UnityEngine;

namespace Tiles
{
    public class RegularTile : MonoBehaviour
    {
        private const float AnomalyProbability = 0.025f;
        private void Start()
        {
            var tilePaletteManager = TilePaletteManager.GetInstance();
            TilePaletteManager.ChangeMaterialColor(gameObject, tilePaletteManager.GetPalette()[2]);
            
            for (var i = 0; i < transform.childCount; i++)
            {
                if (!(Random.value < AnomalyProbability)) continue;
                var innerTile = transform.GetChild(i);
                innerTile.localScale = new Vector3(Random.Range(0.1f, 0.5f), Random.Range(1f, 1.1f), Random.Range(0.1f, 0.5f));
                innerTile.localPosition += new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
            }
        }
    }
}
