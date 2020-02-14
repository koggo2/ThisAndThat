
using System.Collections.Generic;
using TheTile.Game;
using UnityEditor;
using UnityEngine;

namespace TheTile
{
    public class GameHeart : Singleton<GameHeart>
    {
        [SerializeField] private GameGrid _gameGrid;
        [SerializeField] private GameObject _testTilePrefab;

        private Dictionary<Vector3Int, Vector3Int> _allWay;
        private List<Vector3Int> _way;
        
        private float dTime = 1f;

        public void GenerateTileMap()
        {
            for (int x = 0; x < 10; ++x)
            {
                for (int y = 0; y < 10; ++y)
                {
                    var testTileInstance = Instantiate(_testTilePrefab);
                    testTileInstance.transform.SetParent(transform);
                    testTileInstance.name = $"Test Tile {x},{y}";

                    var cellPos = new Vector3Int(x, y, 0);
                    testTileInstance.transform.localPosition = _gameGrid.CellPosToWorld(cellPos);
                }
            }
        }

        private void LateUpdate()
        {
            dTime += Time.deltaTime;
            if (dTime > 1.0f)
            {
                dTime = 0f;

                foreach (var tileData in _gameGrid.TileData)
                {
                    if (tileData.Basement != null)
                    {
                        tileData.Basement.OnBeat_PreUpdateGrid();
                    }
                }
                
                _gameGrid.UpdateGrid();
                
                foreach (var tileData in _gameGrid.TileData)
                {
                    if (tileData.Basement != null)
                    {
                        tileData.Basement.OnBeat_PostUpdateGrid();
                    }
                    // if (tileData.Unit != null)
                    // {
                    //     if (tileData.OnMarch)
                    //     {
                    //         tileData.Unit.SetMarchPosition(tileData.MarchPosition);
                    //     }
                    //     
                    //     tileData.Unit.OnBeat_PostUpdateGrid();
                    // }
                }
            }

#if UNITY_EDITOR
            if (_allWay != null)
            {
                foreach (var cf in _allWay)
                {
                    var key = _gameGrid.CellPosToWorld(cf.Key);
                    var value = _gameGrid.CellPosToWorld(cf.Value);
                    key += new Vector3(0f, 2f, 0f);
                    value += new Vector3(0f, 2f, 0f);
                    
                    Debug.DrawLine(key, value, Color.cyan);
                }
            }
            
            if (_way != null && _way.Count > 1)
            {
                var prev = _way[0];
                foreach (var pos in _way)
                {
                    var a = _gameGrid.CellPosToWorld(prev);
                    var b = _gameGrid.CellPosToWorld(pos);
                    a += new Vector3(0f, 2.1f, 0f);
                    b += new Vector3(0f, 2.1f, 0f);
                    
                    Debug.DrawLine(a, b, Color.red);
                    prev = pos;
                }
            }
#endif
        }

        public void OrderToMove(BaseTile baseTile)
        {
            var aStar = new AStarSearch(_gameGrid, new Vector3Int(0, 0, 0), baseTile.CellPos);
            var index = 0;

            _allWay = aStar.CameFrom;
            _way = aStar.Path;
        }
    }
}
