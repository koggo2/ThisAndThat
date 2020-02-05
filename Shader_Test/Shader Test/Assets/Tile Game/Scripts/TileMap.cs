
using TheTile.Game;
using UnityEngine;

namespace TheTile
{
    public class TileMap : MonoBehaviour
    {
        [SerializeField] private GameGrid _gameGrid;
        [SerializeField] private GameObject _testTilePrefab;
        [SerializeField] private GameObject _testUnitPrefab;
        [SerializeField] private GameObject _testBasementPrefab;
        
        [SerializeField] private Vector3Int _aBaseHouse;
        [SerializeField] private Vector3Int _bBaseHouse;

        private float dTime = 1f;

        public void GenerateTileMap()
        {
            for (int x = 0; x < 10; ++x)
            {
                for (int y = 0; y < 10; ++y)
                {
                    var testTileInstance = Instantiate(_testTilePrefab);
                    testTileInstance.transform.SetParent(transform);

                    var cellPos = new Vector3Int(x, y, 0);
                    testTileInstance.transform.localPosition = _gameGrid.CellPosToVector(cellPos);
                }
            }

            GenerateBasement(_aBaseHouse, BaseObject.TeamEnum.A);
            GenerateBasement(_bBaseHouse, BaseObject.TeamEnum.B);
        }

        private void GenerateBasement(Vector3Int cellPosition, BaseObject.TeamEnum teamEnum)
        {
            var testCubeInstance = Instantiate(_testBasementPrefab);
            testCubeInstance.transform.SetParent(transform);

            testCubeInstance.transform.localPosition = _gameGrid.CellPosToVector(cellPosition);
            
            var basement = testCubeInstance.GetComponent<BaseBasement>();
            basement.Team = teamEnum;
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
                        var unit = tileData.Basement.GenerateUnit(transform);
                        if (unit != null)
                        {
                            var cellPosition = _gameGrid.WorldToCellPos(tileData.Basement.transform.position);
                            unit.transform.localPosition = _gameGrid.CellPosToVector(cellPosition);

                            _gameGrid.AddUnit(cellPosition, unit);
                        }
                    }

                    if (tileData.Unit != null)
                    {
                        var currentCellPosition = _gameGrid.WorldToCellPos(tileData.Unit.transform.position);
                        var destinationCellPosition = currentCellPosition + _gameGrid.GetRandomNeighborTile(currentCellPosition);

                        _gameGrid.MoveUnit(currentCellPosition, destinationCellPosition);
                    }
                }
                
                _gameGrid.UpdateGrid();
            }
        }
    }
}
