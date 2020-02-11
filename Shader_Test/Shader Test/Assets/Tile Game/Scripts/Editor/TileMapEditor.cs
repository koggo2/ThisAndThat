using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TheTile.Editor
{
    public class TileMapEditor : UnityEditor.Editor
    {
        [MenuItem("The Tile/Generate Tile Map 10x10")]
        public static void GenerateTileMap()
        {
            var tileMap = FindObjectOfType<GameHeart>();
            tileMap.GenerateTileMap();
        }
    }
}
