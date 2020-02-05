using UnityEngine;

namespace TheTile.Game
{
    public class BaseBasement : BaseObject
    {
        [SerializeField] private GameObject _testUnitPrefab;

        private int dTurn = 0; 

        private void Awake()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.material.color = Const.GetTeamColor(Team);
            }
        }
        
        public BaseUnit GenerateUnit(Transform parent)
        {
            ++dTurn;

            if (dTurn >= 2)
            {
                dTurn = 0;
                
                var testUnitInstance = Instantiate(_testUnitPrefab);
                testUnitInstance.transform.SetParent(parent);

                var baseUnit = testUnitInstance.GetComponent<BaseUnit>(); 
                baseUnit.SetTeam(Team);
            
                return baseUnit;                
            }

            return null;
        }
    }
}
