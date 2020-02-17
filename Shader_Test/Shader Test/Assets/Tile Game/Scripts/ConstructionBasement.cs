using UnityEngine;

namespace TheTile.Game
{
    public class ConstructionBasement : BaseBasement
    {
        [SerializeField] private GameObject _fx;

        protected override void Awake()
        {
            base.Awake();
            _hp = 0;
        }
        
        public void SetConstructionValue(int value)
        {
            _constructionValue = value;
            UpdateUI();
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
