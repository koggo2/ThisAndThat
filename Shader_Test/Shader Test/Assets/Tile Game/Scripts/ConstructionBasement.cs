using TheTile.Game.Unit;
using TheTile.UI;
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

        protected override void Start()
        {
            base.Start();

            UIManager.Instance.RegisterTargetUI<BaseBasement>(this, ConstData.UI_Cancel);
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

            if (_hp >= _constructionValue && !string.IsNullOrEmpty(_nextBasementName))
            {
                GameController.Instance.CompleteBasement(GameGrid.Instance.WorldToCellPos(transform.position), _nextBasementName);
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
