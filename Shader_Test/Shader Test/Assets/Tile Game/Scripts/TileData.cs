
using System.Collections.Generic;
using System.Linq;
using TheTile.Game;
using UnityEngine;

namespace TheTile
{
    public class TileData
    {
        public Vector3Int Pos;
        public BaseObject.TeamEnum Team;
        public BaseTile Tile;
        public BaseBasement Basement;
        public bool OnMarch = false;
        public Vector3Int MarchPosition;

        public BaseUnit Unit => _unit;
        
        private BaseUnit _unit;
        

        public TileData()
        {
            Pos = Vector3Int.zero;
            Team = BaseObject.TeamEnum.NONE;
            Tile = null;
            _unit = null;
            Basement = null;
            OnMarch = false;
            MarchPosition = Vector3Int.zero;
        }

        public void RemoveUnit()
        {
            _unit = null;
        }

        public void UpdateUnit(BaseUnit newUnit)
        {
            if (_unit == null)
            {
                _unit = newUnit;
            }
            else
            {
                if (_unit.Team == newUnit.Team)
                {
                    _unit.Hp += newUnit.Hp;
                    GameObject.DestroyImmediate(newUnit.gameObject);
                }
                else
                {
                    while (_unit.Hp > 0 && newUnit.Hp > 0)
                    {
                        _unit.Hp -= newUnit.Power;
                        newUnit.Hp -= newUnit.Hp - Unit.Power;                                
                    }

                    if (_unit.Hp <= 0)
                    {
                        GameObject.DestroyImmediate(Unit.gameObject);
                        _unit = null;
                    }

                    if (newUnit.Hp <= 0)
                    {
                        GameObject.DestroyImmediate(newUnit.gameObject);
                        newUnit = null;
                    }

                    if (_unit == null && newUnit != null)
                    {
                        _unit = newUnit;
                    }
                }
            }
        }
    }
}