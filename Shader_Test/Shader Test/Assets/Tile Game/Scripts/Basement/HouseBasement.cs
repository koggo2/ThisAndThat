using System.Collections.Generic;
using System.Linq;
using TheTile.Game.Unit;
using UnityEngine;

namespace TheTile.Game
{
    public class HouseBasement : BaseBasement
    {
        public List<BaseUnit> GeneratedUnits => _generatedUnits;

        private int dTurn = 0;
        private List<BaseUnit> _generatedUnits = new List<BaseUnit>();
        
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
            if(tileData.APlaceToGo.Count > 0 && tileData.Unit != null)
            {
                var toGoPos = tileData.APlaceToGo.First();
                var unit = tileData.Unit;
                unit.March(new AStarSearch(GameGrid.Instance, tileData.Pos, toGoPos));
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
                baseUnit.OriginBasement = this;
                baseUnit.SetTeam(Team);

                _generatedUnits.Add(baseUnit);
            
                return baseUnit;
            }

            return null;
        }

        public void UnlinkGeneratedUnit(BaseUnit unit)
        {
            if (_generatedUnits.Contains(unit))
            {
                _generatedUnits.Remove(unit);
            }
        }
    }
}
