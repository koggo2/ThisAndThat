using TheTile.Game.Unit;
using UnityEngine;

namespace TheTile.Game
{
    public class ConstructionBasement : BaseBasement
    {
        [SerializeField] private GameObject _fx;

        private string _nextBasementName;

        protected override void Awake()
        {
            base.Awake();
            _hp = 0;
        }
        
        public void SetConstructionData(string nextBasementName)
        {
            _nextBasementName = nextBasementName;
            var nextBasementPrefab = Resources.Load<GameObject>(nextBasementName);
            if (nextBasementPrefab != null)
            {
                var prefabBasementComponent = nextBasementPrefab.GetComponent<BaseBasement>();
                if (prefabBasementComponent != null)
                {
                    _constructionValue = prefabBasementComponent.ConstructionValue;
                }
            }
        }
        
        public override void OnBeat_PostUpdateGrid()
        {
            var tileData = GameGrid.Instance.GetUnderTileData(this);

            if(tileData.Unit != null)
            {
                AbsorbUnit(tileData.Unit);
                tileData.RemoveUnit(true);
            }
            
            base.OnBeat_PostUpdateGrid();

            if (_hp >= _constructionValue)
            {
                GameGrid.Instance.BuildTile(tileData.Pos, "Empty Tile", Team);
                var houseBasement = GameGrid.Instance.BuildBasement<HouseBasement>(tileData.Pos, "House Basement", Team);
                Debug.Log($"Construction Complete :: {houseBasement.Hp} // {houseBasement.ConstructionValue}");
            }
        }

        private void AbsorbUnit(BaseUnit unit)
        {
            if (unit == null)
                return;
            
            _hp += unit.Hp;
        }
    }
}
