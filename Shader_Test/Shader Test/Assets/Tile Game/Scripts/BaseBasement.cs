using UnityEngine;

namespace TheTile.Game
{
    public class BaseBasement : BaseObject
    {
        [SerializeField] private GameObject _testUnitPrefab;

        private void Awake()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.material.color = Const.GetTeamColor(Team);
            }
        }
        
        public BaseUnit GenerateUnit(Grid grid, Transform parent)
        {
            var testUnitInstance = Instantiate(_testUnitPrefab);
            testUnitInstance.transform.SetParent(parent);

            var cellPosition = grid.WorldToCell(transform.position);
            testUnitInstance.transform.localPosition = grid.GetCellCenterLocal(cellPosition);

            var baseUnit = testUnitInstance.GetComponent<BaseUnit>(); 
            baseUnit.SetTeam(Team);
            
            return baseUnit;
        }
    }
}
