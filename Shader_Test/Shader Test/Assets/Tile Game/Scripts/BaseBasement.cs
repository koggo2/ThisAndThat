using UnityEngine;

namespace TheTile.Game
{
    public class BaseBasement : MonoBehaviour
    {
        [SerializeField] private GameObject _testUnitPrefab;
        
        public GameObject GenerateUnit(Grid grid, Transform parent)
        {
            var testCubeInstance = Instantiate(_testUnitPrefab);
            testCubeInstance.transform.SetParent(parent);

            var cellPosition = grid.WorldToCell(transform.position);
            testCubeInstance.transform.localPosition = grid.GetCellCenterLocal(cellPosition);
        }
    }
}
