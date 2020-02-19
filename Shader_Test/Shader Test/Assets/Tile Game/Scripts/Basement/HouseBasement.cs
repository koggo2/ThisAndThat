using TheTile.Game.Unit;
using UnityEngine;

namespace TheTile.Game
{
    public class HouseBasement : BaseBasement
    {
        private int dTurn = 0;
        
        public override void OnBeat_PreUpdateGrid()
        {
            var generatedUnit = GenerateUnit();
            if (generatedUnit != null)
            {
                GameGrid.Instance.AttachUnit(this, generatedUnit);
            }
        }

        public override void OnBeat_PostUpdateGrid()
        {
            base.OnBeat_PostUpdateGrid();

            var tileData = GameGrid.Instance.GetUnderTileData(this);
            if(tileData.OnMarch && tileData.Unit != null)
            {
                var unit = tileData.Unit;
                unit.March(new AStarSearch(GameGrid.Instance, tileData.Pos, tileData.MarchPosition));
            }
        }

        public BaseUnit GenerateUnit()
        {
            ++dTurn;

            if (dTurn >= 2)
            {
                dTurn = 0;

                GameObject prefab = null;
                if (Team == TeamEnum.A)
                {
                    prefab = Resources.Load<GameObject>(ConstData.Unit_WorkerPrefabPath);
                }
                else
                {
                    prefab = Resources.Load<GameObject>(ConstData.Unit_WorkerPrefabPath);
                }

                if (prefab == null)
                {
                    Debug.LogError("Unit Prefab is null..!");
                    return null;
                }
                
                var testUnitInstance = Instantiate(prefab);
                testUnitInstance.transform.localScale = Vector3.one;
                
                var baseUnit = testUnitInstance.GetComponent<BaseUnit>(); 
                baseUnit.SetTeam(Team);
            
                return baseUnit;                
            }

            return null;
        }
    }
}
