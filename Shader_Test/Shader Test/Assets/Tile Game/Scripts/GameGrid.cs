using System.Collections.Generic;
using System.Linq;
using TheTile.Game;
using TheTile.Game.Unit;
using UnityEngine;

namespace TheTile
{
    public class GameGrid : Singleton<GameGrid>, WeightedGraph<Vector3Int>
    {
        public static readonly Vector3Int[,] DIRS = new Vector3Int[,]
        {
            // Even row
            {
                new Vector3Int(-1, -1, 0),
                new Vector3Int(0, -1, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(-1, 1, 0),
                new Vector3Int(-1, 0, 0),
            },
            // Odd row
            {
                new Vector3Int(0, -1, 0),
                new Vector3Int(1, -1, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(1, 1, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(-1, 0, 0),                
            }
        };
        
        [SerializeField] private Grid _grid;

        public List<TileData> TileData => _gridMap.Values.ToList();
        private Dictionary<Vector3Int, TileData> _gridMap;

        private void Awake()
        {
            _gridMap = new Dictionary<Vector3Int, TileData>();
            
            var tiles = FindObjectsOfType<BaseTile>();
            foreach (var baseTile in tiles)
            {
                var cellPos = _grid.WorldToCell(baseTile.transform.position);
                var tileData = new TileData()
                {
                    Pos = cellPos,
                    Tile = baseTile,
                };
                tileData.Tile.CellPos = cellPos;
                
                _gridMap.Add(cellPos, tileData);
            }

            var basements = FindObjectsOfType<BaseBasement>();
            foreach (var basement in basements)
            {
                var cellPos = _grid.WorldToCell(basement.transform.position);
                if (_gridMap.ContainsKey(cellPos))
                {
                    _gridMap[cellPos].UpdateBasement(basement);
                    _gridMap[cellPos].Team = basement.Team;
                    _gridMap[cellPos].Tile.SetTeam(basement.Team);
                }
            }
        }

        public Vector3 CellPosToWorld(Vector3Int cellPos)
        {
            return _grid.GetCellCenterLocal(cellPos);
        }

        public Vector3Int WorldToCellPos(Vector3 worldPos)
        {
            return _grid.WorldToCell(worldPos);
        }

        public void AttachUnit(BaseBasement basement, BaseUnit unit)
        {
            var cellPos = WorldToCellPos(basement.transform.position);
            AttachUnit(cellPos, unit);
        }

        public void AttachUnit(Vector3Int cellPos, BaseUnit unit, bool resetTransform = true)
        {
            // Debug.Log($"Add Unit, Pos = {cellPos}");
            if (!HasCellPos(cellPos)) return;

            var gridData = _gridMap[cellPos];
            unit.transform.SetParent(gridData.Tile.Pivot);
            if(resetTransform)
            {
                unit.transform.localPosition = Vector3.zero;
                unit.transform.localRotation = Quaternion.identity;
                unit.transform.localScale = Vector3.one;
            }

            gridData.UpdateUnit(unit);
        }
        
        public void DetachUnit(Vector3Int cellPos)
        {
            if (!HasCellPos(cellPos)) return;
            
            var gridData = _gridMap[cellPos];
            
            gridData.RemoveUnit();
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
            var parity = pos.y & 1;
            for (int i = 0; i < 6; ++i)
            {
                var dir = DIRS[parity, i];
                var next = new Vector3Int(pos.x + dir.x, pos.y + dir.y, pos.z + dir.z);
                if (Possible(next))
                {
                    yield return next;
                }
            }
        }

        public TileData GetUnderTileData(BaseObject baseObject)
        {
            var cellPos = WorldToCellPos(baseObject.transform.position);

            return GetTileData(cellPos);
        }

        public TileData GetTileData(Vector3Int cellPos)
        {
            if (!HasCellPos(cellPos)) return null;

            return _gridMap[cellPos];
        }

        public bool HasCellPos(Vector3Int cellPos)
        {
            if (!_gridMap.ContainsKey(cellPos))
            {
                Debug.LogError($"Set March Error :: {cellPos} has no grid data..!");
                return false;
            }

            return true;
        }
    }
}
