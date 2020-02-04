
using System;
using System.Collections;
using System.Collections.Generic;
using TheTile.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheTile
{
    [Serializable]
    public class TileData
    {
        public BaseObject.TeamEnum Team;
        public BaseTile Tile;
        public BaseUnit Unit;
        public BaseBasement Basement;

        public List<BaseUnit> TemporaryUnit;

        public TileData()
        {
            Team = BaseObject.TeamEnum.NONE;
            Tile = null;
            Unit = null;
            Basement = null;
            TemporaryUnit = new List<BaseUnit>();
        }

        public void Update()
        {
            if (Unit != null)
            {
                GameObject.DestroyImmediate(Unit.gameObject);
                Unit = null;
            }
            
            if (Tile != null && TemporaryUnit != null && TemporaryUnit.Count > 0)
            {
                Unit = TemporaryUnit[0];

                for (var i = 1; i < TemporaryUnit.Count; ++i)
                {
                    var nextUnit = TemporaryUnit[i];
                    if (Unit == null)
                    {
                        Unit = nextUnit;
                    }
                    else
                    {
                        if (Unit.Team == nextUnit.Team)
                        {
                            Unit.Hp += nextUnit.Hp;
                        }
                        else
                        {
                            while (Unit.Hp > 0 && nextUnit.Hp > 0)
                            {
                                Unit.Hp -= nextUnit.Power;
                                nextUnit.Hp -= nextUnit.Hp - Unit.Power;                                
                            }

                            if (Unit.Hp <= 0)
                            {
                                GameObject.DestroyImmediate(Unit.gameObject);
                                Unit = null;
                            }

                            if (nextUnit.Hp <= 0)
                            {
                                GameObject.DestroyImmediate(nextUnit.gameObject);
                                nextUnit = null;
                            }

                            if (Unit == null && nextUnit != null)
                            {
                                Unit = nextUnit;
                            }
                        }
                    }
                }
                
                TemporaryUnit.Clear();
            }
            
            if(Unit != null)
                Tile.SetTeam(Unit.Team);
        }

        public void AddUnit(BaseUnit unit)
        {
            TemporaryUnit.Add(unit);
        }

        public void MoveUnit(Vector3 getCellCenterWorld)
        {
            Unit.Move(getCellCenterWorld);
            Unit = null;
        }

        public void GenerateUnit(Grid _grid, Transform transform)
        {
            if (Basement != null)
            {
                var baseUnit = Basement.GenerateUnit(_grid, transform);
                TemporaryUnit.Add(baseUnit);
            }
        }
    }
    
    public class TileMap : MonoBehaviour
    {
        public static Vector3Int TOP_LEFT = new Vector3Int(-1, -1, 0);
        public static Vector3Int TOP_RIGHT = new Vector3Int(0, -1, 0);
        public static Vector3Int RIGHT = new Vector3Int(1, 0, 0);
        public static Vector3Int BOTTOM_RIGHT = new Vector3Int(0, 1, 0);
        public static Vector3Int BOTTOM_LEFT = new Vector3Int(-1, 1, 0);
        public static Vector3Int LEFT = new Vector3Int(-1, 0, 0);
        
        [SerializeField] private Grid _grid;
        [SerializeField] private GameObject _testTilePrefab;
        [SerializeField] private GameObject _testUnitPrefab;
        [SerializeField] private GameObject _testBasementPrefab;
        
        [SerializeField] private Vector3Int _aBaseHouse;
        [SerializeField] private Vector3Int _bBaseHouse;

        [SerializeField] private Dictionary<Vector3Int, TileData> _gridMap;

        private float dTime = 1f;

        private void Awake()
        {
            _gridMap = new Dictionary<Vector3Int, TileData>();
            
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
                }
            }
        }

        public void GenerateTileMap()
        {
            for (int x = 0; x < 10; ++x)
            {
                for (int y = 0; y < 10; ++y)
                {
                    var testTileInstance = Instantiate(_testTilePrefab);
                    testTileInstance.transform.SetParent(transform);

                    var cellPos = new Vector3Int(x, y, 0);
                    testTileInstance.transform.localPosition = _grid.GetCellCenterLocal(cellPos);
                }
            }

            GenerateBasement(_aBaseHouse, BaseObject.TeamEnum.A);
            GenerateBasement(_bBaseHouse, BaseObject.TeamEnum.B);
        }

        private void GenerateBasement(Vector3Int cellPosition, BaseObject.TeamEnum teamEnum)
        {
            var testCubeInstance = Instantiate(_testBasementPrefab);
            testCubeInstance.transform.SetParent(transform);

            testCubeInstance.transform.localPosition = _grid.GetCellCenterLocal(cellPosition);
            
            var basement = testCubeInstance.GetComponent<BaseBasement>();
            basement.Team = teamEnum;

            if (_gridMap.ContainsKey(cellPosition))
            {
                var data = _gridMap[cellPosition];
                data.Basement = basement;
                data.Tile.SetTeam(basement.Team);
            }
        }

        private void LateUpdate()
        {
            dTime += Time.deltaTime;
            if (dTime > 2.0f)
            {
                dTime = 0f;
                
                foreach (var gridMapValue in _gridMap.Values)
                {
                    if (gridMapValue.Basement != null)
                    {
                        gridMapValue.GenerateUnit(_grid, transform);
                    }

                    if (gridMapValue.Unit != null)
                    {
                        var currentCellPosition = _grid.WorldToCell(gridMapValue.Unit.transform.position);
                        var destinationCellPosition = currentCellPosition + GetRandomNeighborTile(currentCellPosition);

                        var targetGridMap = _gridMap[destinationCellPosition];
                        targetGridMap.AddUnit(gridMapValue.Unit);
                        
                        gridMapValue.MoveUnit(_grid.GetCellCenterWorld(destinationCellPosition));
                    }
                }

                foreach (var gridMapValue in _gridMap.Values)
                {
                    gridMapValue.Update();
                }
            }
        }

        private Vector3Int GetRandomNeighborTile(Vector3Int originPos)
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
