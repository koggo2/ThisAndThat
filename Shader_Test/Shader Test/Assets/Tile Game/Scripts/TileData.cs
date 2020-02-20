using System.Collections.Generic;
using TheTile.Game;
using TheTile.Game.Unit;
using UnityEngine;

namespace TheTile
{
    public class TileData
    {
        public Vector3Int Pos;
        public BaseObject.TeamEnum Team;
        public BaseTile Tile;
        
        public bool OnConstruction = false;
        public List<Vector3Int> APlaceToGo = new List<Vector3Int>();
        public List<Vector3Int> APlaceToCome = new List<Vector3Int>();

        public BaseUnit Unit => _unit;
        public BaseBasement Basement => _basement;
        
        private BaseUnit _unit;
        private BaseBasement _basement;

        public TileData()
        {
            Pos = Vector3Int.zero;
            Team = BaseObject.TeamEnum.NONE;
            Tile = null;
            _unit = null;
            _basement = null;
            OnConstruction = false;
        }

        public void UpdateBasement(BaseBasement basement)
        {
            if (_basement != null)
            {
                GameObject.DestroyImmediate(_basement.gameObject);
            }
            
            _basement = basement;
        }

        public void RemoveBasement()
        {
            UpdateBasement(null);
        }

        public void RemoveUnit(bool destroyUnit = false)
        {
            if (destroyUnit)
            {
                GameObject.DestroyImmediate(_unit.gameObject);
            }
            
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
                        newUnit.Hp -= _unit.Power;                                
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