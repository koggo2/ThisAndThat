using System;
using TheTile.Util;
using UnityEngine;

namespace TheTile.Game
{
    public class BaseBasement : BaseObject
    {
        private int dTurn = 0;

        public virtual void OnBeat()
        {
            var generatedUnit = GenerateUnit();
            if (generatedUnit != null)
            {
                GameGrid.Instance.AddUnit(this, generatedUnit);
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
                var baseUnit = testUnitInstance.GetComponent<BaseUnit>(); 
                baseUnit.SetTeam(Team);
            
                return baseUnit;                
            }

            return null;
        }

        public void OnMouseDown()
        {
            Debug.Log("On Mouse Down");
        }

        public void OnMouseUp()
        {
            Debug.Log("On Mouse Up");
            LineManager.Instance.HideLine();
        }

        public void OnMouseDrag()
        {
            if (SelectingObjects.MouseOveredTile != null)
            {
                LineManager.Instance.DrawArc(GameGrid.Instance.WorldToCellPos(transform.position), GameGrid.Instance.WorldToCellPos(SelectingObjects.MouseOveredTile.transform.position));                
            }
        }
    }
}
