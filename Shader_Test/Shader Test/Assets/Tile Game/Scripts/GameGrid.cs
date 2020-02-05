using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheTile.Game;
using UnityEngine;

namespace TheTile
{
    public class GameGrid : MonoBehaviour, WeightedGraph<Vector3Int>
    {
        public static Vector3Int TOP_LEFT = new Vector3Int(-1, -1, 0);
        public static Vector3Int TOP_RIGHT = new Vector3Int(0, -1, 0);
        public static Vector3Int RIGHT = new Vector3Int(1, 0, 0);
        public static Vector3Int BOTTOM_RIGHT = new Vector3Int(0, 1, 0);
        public static Vector3Int BOTTOM_LEFT = new Vector3Int(-1, 1, 0);
        public static Vector3Int LEFT = new Vector3Int(-1, 0, 0);

        public static readonly Vector3Int[] DIRS = new[]
        {
            new Vector3Int(-1, -1, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(-1, 0, 0),
        };
        
        [SerializeField] private Grid _grid;

        public List<TileData> TileData => _gridMap.Values.ToList();
        
        private Dictionary<Vector3Int, TileData> _gridMap;
        private List<BaseBasement> _basements;
        private List<BaseUnit> _units;

        private void Awake()
        {
            _gridMap = new Dictionary<Vector3Int, TileData>();
            _basements = new List<BaseBasement>();
            _units = new List<BaseUnit>();
            
            var tiles = FindObjectsOfType<BaseTile>();
            foreach (var baseTile in tiles)
            {
                var cellPos = _grid.WorldToCell(baseTile.transform.position);
                _gridMap.Add(cellPos, new TileData()
                {
                    Tile = baseTile,
                });
            }

            var basements = FindObjectsOfType<BaseBasement>();
            foreach (var basement in basements)
            {
                var cellPos = _grid.WorldToCell(basement.transform.position);
                if (_gridMap.ContainsKey(cellPos))
                {
                    _gridMap[cellPos].Basement = basement;
                    _gridMap[cellPos].Team = basement.Team;
                    _gridMap[cellPos].Tile.SetTeam(basement.Team);
                }

                _basements.Add(basement);
            }
        }

        public Vector3 CellPosToVector(Vector3Int cellPos)
        {
            return _grid.GetCellCenterLocal(cellPos);
        }

        public Vector3Int WorldToCellPos(Vector3 worldPos)
        {
            return _grid.WorldToCell(worldPos);
        }

        public void AddUnit(Vector3Int cellPos, BaseUnit unit)
        {
            if (!_gridMap.ContainsKey(cellPos))
            {
                _gridMap.Add(cellPos, new TileData());
            }
            
            _gridMap[cellPos].TemporaryUnit.Add(unit);
        }

        public void MoveUnit(Vector3Int currentPos, Vector3Int destination)
        {
            if (!_gridMap.ContainsKey(currentPos))
            {
                _gridMap.Add(currentPos, new TileData());
            }

            var currentMapData = _gridMap[currentPos];
            if (currentMapData.Unit != null)
            {
                var unit = currentMapData.Unit;
                currentMapData.Unit = null;

                unit.Move(_grid.GetCellCenterWorld(destination));

                AddUnit(destination, unit);
            }
        }

        public void UpdateGrid()
        {
            foreach (var gridValue in _gridMap.Values)
            {
                gridValue.Update();
            }
        }

        public bool Possible(Vector3Int pos)
        {
            if (_gridMap.ContainsKey(pos))
            {
                return true;
            }

            return false;
        }

        public int Cost(Vector3Int a, Vector3Int b)
        {
            return 1;
        }

        public IEnumerable<Vector3Int> Neighbors(Vector3Int pos)
        {
            foreach (var dir in DIRS)
            {
                var next = new Vector3Int(pos.x + dir.x, pos.y + dir.y, pos.z + dir.z);
                if (Possible(next))
                {
                    yield return next;
                }
            }
        }
        
        public Vector3Int GetRandomNeighborTile(Vector3Int originPos)
        {
            List<Vector3Int> directions = new List<Vector3Int>();
            if (_gridMap.ContainsKey(originPos + TOP_LEFT))
            {
                directions.Add(TOP_LEFT);
            }
            if (_gridMap.ContainsKey(originPos + TOP_RIGHT))
            {
                directions.Add(TOP_RIGHT);
            }
            if (_gridMap.ContainsKey(originPos + RIGHT))
            {
                directions.Add(RIGHT);
            }
            if (_gridMap.ContainsKey(originPos + BOTTOM_RIGHT))
            {
                directions.Add(BOTTOM_RIGHT);
            }
            if (_gridMap.ContainsKey(originPos + BOTTOM_LEFT))
            {
                directions.Add(BOTTOM_LEFT);
            }
            if (_gridMap.ContainsKey(originPos + LEFT))
            {
                directions.Add(LEFT);
            }

            return directions[Random.Range(0, directions.Count)];
        }
    }
}
