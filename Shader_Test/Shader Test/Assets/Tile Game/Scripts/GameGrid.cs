using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheTile.Game;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        private List<BaseBasement> _basements;

        private void Awake()
        {
            _gridMap = new Dictionary<Vector3Int, TileData>();
            _basements = new List<BaseBasement>();
            
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

                _basements.Add(basement);
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
        
        public Vector3Int GetRandomNeighborTile(Vector3Int originPos)
        {
            var parity = originPos.y & 1;
            List<Vector3Int> directions = new List<Vector3Int>();
            if (_gridMap.ContainsKey(originPos + DIRS[parity, 0]))
            {
                directions.Add(DIRS[parity, 0]);
            }
            if (_gridMap.ContainsKey(originPos + DIRS[parity, 1]))
            {
                directions.Add(DIRS[parity, 1]);
            }
            if (_gridMap.ContainsKey(originPos + DIRS[parity, 2]))
            {
                directions.Add(DIRS[parity, 2]);
            }
            if (_gridMap.ContainsKey(originPos + DIRS[parity, 3]))
            {
                directions.Add(DIRS[parity, 3]);
            }
            if (_gridMap.ContainsKey(originPos + DIRS[parity, 4]))
            {
                directions.Add(DIRS[parity, 4]);
            }
            if (_gridMap.ContainsKey(originPos + DIRS[parity, 5]))
            {
                directions.Add(DIRS[parity, 5]);
            }

            return directions[Random.Range(0, directions.Count)];
        }

        public void SetMarch(HouseBasement houseBasement)
        {
            var cellPos = WorldToCellPos(houseBasement.transform.position);
            if (!HasCellPos(cellPos)) return;

            var selectedCellPos = SelectingObjects.MouseOveredCellPos;

            var tileData = _gridMap[cellPos];

            if (cellPos == selectedCellPos)
            {
                tileData.OnMarch = false;
            }
            else
            {
                tileData.OnMarch = true;
                tileData.MarchPosition = selectedCellPos;
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

        public T BuildBasement<T>(Vector3Int cellPos, string prefabName, BaseObject.TeamEnum team) where T : BaseBasement
        {
            if (!HasCellPos(cellPos)) return null;
            
            var tileData = _gridMap[cellPos];
            
            var prefab = Resources.Load<GameObject>(prefabName);
            var prefabInstance = Instantiate(prefab);
            prefabInstance.transform.SetParent(tileData.Tile.Pivot);
            prefabInstance.transform.localPosition = Vector3.zero;
            
            var baseBasement = prefabInstance.GetComponent<BaseBasement>();
            baseBasement.SetTeam(team);
            tileData.UpdateBasement(baseBasement);

            return baseBasement as T;
        }
        
        public void BuildTile(Vector3Int cellPos, string tilePrefabName, BaseObject.TeamEnum team)
        {
            if (!HasCellPos(cellPos)) return;
            
            var tileData = _gridMap[cellPos];

            var prefab = Resources.Load<GameObject>(tilePrefabName);
            var prefabInstance = Instantiate(prefab);
            prefabInstance.transform.SetParent(tileData.Tile.transform.parent);
            prefabInstance.transform.localPosition = tileData.Tile.transform.localPosition;
            
            DestroyImmediate(tileData.Tile.gameObject);
            
            var tile = prefabInstance.GetComponent<BaseTile>();
            tile.SetTeam(team);
            tileData.Tile = tile;
        }

        private bool HasCellPos(Vector3Int cellPos)
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
