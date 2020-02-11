﻿using System.Collections;
using System.Collections.Generic;
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

        public override void OnMouseUp()
        {
            base.OnMouseUp();

            GameGrid.Instance.SetMarch(this);
        }
    }
}