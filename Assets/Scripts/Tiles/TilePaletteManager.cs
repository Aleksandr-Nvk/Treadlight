    using System.Linq;
    using UnityEngine;
    using Random = UnityEngine.Random;

    namespace Tiles
    {
        public class TilePaletteManager
        {
            private static TilePaletteManager _instance;

            private static string _palettesRaw;
            private static Color[] _palette;

            private const int PalettesCount = 462; // very hardcoded value. Predefined in Resources/palettes.txt
            private static int _seed;
                
            public static TilePaletteManager GetInstance()
            {
                if (_instance is not null) return _instance;
                
                var textFile = Resources.Load<TextAsset>("palettes");
                var fileContent = textFile?.text;
                _palettesRaw = fileContent;
                _seed = Random.Range(1, PalettesCount + 1);
                
                return _instance = new TilePaletteManager();
            }

            public Color[] GetPalette()
            {
                if (_palette is not null) return _palette;
                
                var startIndex = _palettesRaw.IndexOf($"{_seed}=");
                startIndex = _palettesRaw.IndexOf('\n', startIndex);
                var currentIndex = startIndex + 1;
                var newlineCount = 0;
                var endIndex = currentIndex;
                while (endIndex < _palettesRaw.Length && newlineCount < 4)
                {
                    if (_palettesRaw[endIndex] == '\n')
                    {
                        newlineCount++;
                    }
                    
                    endIndex++;
                }

                var paletteRaw = _palettesRaw.Substring(currentIndex, endIndex - currentIndex);
                var colorsRaw = paletteRaw.Split(',');
                var colorPalette = colorsRaw.Select(colorRaw =>
                {
                    ColorUtility.TryParseHtmlString(colorRaw, out var color);
                    return color;
                }).ToArray();
                
                return _palette ??= colorPalette;
            }
            
            public static void ChangeMaterialColor(GameObject gameObject, Color color)
            {
                var renderer = gameObject.GetComponent<Renderer>();
                renderer.material.color = color;

                foreach (var childRenderer in gameObject.GetComponentsInChildren<Renderer>())
                {
                    Debug.Log(childRenderer.material.shader.name);
                    if (childRenderer.material.shader.name != "Particles")
                    {
                        
                    }
                    childRenderer.material.color = color;
                }
            }
            
        }
    }
