    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Random = UnityEngine.Random;

    namespace Tiles
    {
        public class TilePaletteManager
        {
            private static List<Color> _palette;

            private static TilePaletteManager _instance;

            private static string _palettesRaw;

            private const int PalettesCount = 462;
            private static int _seed;
                
            public static TilePaletteManager GetInstance()
            {
                if (_instance is not null) return _instance;
                
                var textFile = Resources.Load<TextAsset>("colors");
                var fileContent = textFile?.text;
                _palettesRaw = fileContent;
                _seed = Random.Range(1, PalettesCount + 1);
                
                return _instance = new TilePaletteManager();
            }

            public List<Color> GetPalette()
            {
                if (_palette is not null) return _palette;
                
                var startIndex = _palettesRaw.IndexOf($"{_seed}", StringComparison.Ordinal);
                if (startIndex == -1)
                {
                    Debug.Log("Unique number not found.");
                    return null;
                }

                startIndex = _palettesRaw.IndexOf('\n', startIndex);
                if (startIndex == -1)
                {
                    Debug.Log("No newline after unique number.");
                    return null;
                }

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

                var chunk = _palettesRaw.Substring(currentIndex, endIndex - currentIndex);
                var colorsRaw = chunk.Split(',');
                var colorPalette = colorsRaw.Select(colorRaw =>
                {
                    ColorUtility.TryParseHtmlString(colorRaw, out var color);
                    return color;
                }).ToList();
                
                return _palette ??= colorPalette;
            }
            
            public static void ChangeMaterialColor(GameObject gameObject, Color color)
            {
                var renderer = gameObject.GetComponent<Renderer>();
                renderer.material.color = color;

                foreach (var childRenderer in gameObject.GetComponentsInChildren<Renderer>())
                {
                    childRenderer.material.color = color;
                }
            }
            
        }
    }
