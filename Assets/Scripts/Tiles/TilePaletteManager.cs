    using System.Collections.Generic;
    using UnityEngine;

    namespace Tiles
    {
        public static class TilePaletteManager
        {
            private static List<Color> _palette;
            
            public static void ChangeMaterialColor(GameObject gameObject, Color color)
            {
                var renderer = gameObject.GetComponent<Renderer>();
                renderer.material.color = color;

                foreach (var childRenderer in gameObject.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = color;
                }
            }
            
            private static Color GeneratePastelColor()
            {
                return Color.HSVToRGB(Random.value, 0.2f, 0.95f);
            }
            
            public static List<Color> GetRandomPalette()
            {
                if (_palette is not null)
                {
                    return _palette;
                }
                
                var colors = new List<Color>();
                for (var i = 0; i < 4; i++)
                {
                    colors.Add(GeneratePastelColor());
                }

                _palette = colors;
                return colors;
                
            }
        }
    }
