using TheTile.UI;
using TheTile.Util;
using TMPro;
using UnityEngine;

namespace TheTile.Game
{
    public class BaseBasement : BaseObject
    {
        [SerializeField] protected int _constructionValue;
        [SerializeField] protected int _hp;

        public int Hp => _hp;
        public int ConstructionValue => _constructionValue;

        protected virtual void Awake()
        {
            _hp = _constructionValue;
        }

        protected virtual void Start()
        {
            UIManager.Instance.RegisterBasementInfoUI(this);
        }
        
        public override void OnBeat_PostUpdateGrid()
        {
            base.OnBeat_PostUpdateGrid();
        }

        public void OnMouseDown()
        {
            SelectingObjects.SelectedBasement = this;
        }

        public virtual void OnMouseUp()
        {
            SelectingObjects.SelectedBasement = null;
            LineManager.Instance.HideLine();
        }

        public virtual void OnMouseDrag()
        {
            var mouseOveredTileCellPos = SelectingObjects.MouseOveredCellPos;
            var basementCellPos = GameGrid.Instance.WorldToCellPos(transform.position);

            if (mouseOveredTileCellPos != basementCellPos)
            {
                LineManager.Instance.DrawArc(basementCellPos, mouseOveredTileCellPos);                    
            }
        }
    }
}
