using TheTile.Util;
using TMPro;
using UnityEngine;

namespace TheTile.Game
{
    public class BaseBasement : BaseObject
    {
        [SerializeField] protected int _constructionValue;
        [SerializeField] protected int _hp;
        [SerializeField] private TextMeshPro _textMesh;

        protected virtual void Awake()
        {
            _hp = _constructionValue;
        }
        
        public override void OnBeat_PostUpdateGrid()
        {
            base.OnBeat_PostUpdateGrid();

            UpdateUI();
        }

        protected void UpdateUI()
        {
            _textMesh.text = $"{_hp} / {_constructionValue}";   
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
