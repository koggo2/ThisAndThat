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

        public void SetUnit(BaseUnit unit)
        {
            if (Unit != null)
            {
                var pos = GameGrid.Instance.WorldToCellPos(unit.transform.position);
                Debug.LogWarning($"{pos}, Already has unit..!");
                return;
            }

            _unit = unit;
        }

        public void RemoveUnit(bool destroyUnit = false)
        {
            if (destroyUnit)
            {
                GameObject.Destroy(_unit.gameObject);
            }
            
            _unit = null;
        }
    }
}