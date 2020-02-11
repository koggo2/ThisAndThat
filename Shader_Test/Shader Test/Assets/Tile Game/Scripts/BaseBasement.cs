using UnityEngine;

namespace TheTile.Game
{
    public class BaseBasement : BaseObject
    {
        private int dTurn = 0; 

        private void Awake()
        {
            
        }
        
        public BaseUnit GenerateUnit(Transform parent)
        {
            ++dTurn;

            if (dTurn >= 2)
            {
                dTurn = 0;

                GameObject prefab = null;
                if (Team == TeamEnum.A)
                {
                    prefab = Resources.Load<GameObject>("Test Team A");
                }
                else
                {
                    prefab = Resources.Load<GameObject>("Test Team B");
                }

                if (prefab == null)
                {
                    Debug.LogError("Unit Prefab is null..!");
                    return null;
                }
                
                var testUnitInstance = Instantiate(prefab);
                testUnitInstance.transform.SetParent(parent);

                var baseUnit = testUnitInstance.GetComponent<BaseUnit>(); 
                baseUnit.SetTeam(Team);
            
                return baseUnit;                
            }

            return null;
        }
    }
}
