
using System;
using System.Collections.Generic;
using TheTile.Game;
using UnityEngine;

namespace TheTile
{
    public class TileMap : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private GameObject _testTilePrefab;
        [SerializeField] private List<GameObject> _testTileList;
        [SerializeField] private GameObject _testUnitPrefab;
        [SerializeField] private List<GameObject> _testUnitList;
        [SerializeField] private GameObject _testBasementPrefab;
        [SerializeField] private List<BaseBasement> _testBasementList;
        
        [SerializeField] private Vector3Int _aBaseHouse;
        [SerializeField] private Vector3Int _bBaseHouse;

        private float dTime = 1f;

        public void GenerateTileMap()
        {
            if(_testTileList == null)
                _testTileList = new List<GameObject>();
            _testTileList.ForEach(cube => { DestroyImmediate(cube.gameObject); });
            _testTileList.Clear();
            
            for (int x = 0; x < 10; ++x)
            {
                for (int y = 0; y < 10; ++y)
                {
                    var testCubeInstance = Instantiate(_testTilePrefab);
                    testCubeInstance.transform.SetParent(transform);

                    testCubeInstance.transform.localPosition = _grid.GetCellCenterLocal(new Vector3Int(x, y, 0));

                    _testTileList.Add(testCubeInstance);
                }
            }

            GenerateBasement(_aBaseHouse);
            GenerateBasement(_bBaseHouse);
        }

        private void GenerateBasement(Vector3Int cellPosition)
        {
            var testCubeInstance = Instantiate(_testBasementPrefab);
            testCubeInstance.transform.SetParent(transform);

            testCubeInstance.transform.localPosition = _grid.GetCellCenterLocal(cellPosition);

            _testBasementList.Add(testCubeInstance.GetComponent<BaseBasement>());
        }

        private void LateUpdate()
        {
            dTime += Time.deltaTime;
            if (dTime > 2.0f)
            {
                dTime = 0f;
                _testBasementList.ForEach(basement => basement.GenerateUnit(_grid, transform));
            }
        }
    }
}
