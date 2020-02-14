using TheTile.Util;
using TMPro;
using UnityEngine;

namespace TheTile.Game
{
    public class BaseBasement : BaseObject
    {
        [SerializeField] private int _constructionValue;
        [SerializeField] private int _hp;
        [SerializeField] private TextMeshPro _textMesh;

        protected virtual void Awake()
        {
            _hp = _constructionValue;
        }
        
        public override void OnBeat_PostUpdateGrid()
        {
            base.OnBeat_PostUpdateGrid();

            _textMesh.text = $"{_hp} / {_constructionValue}";
        }
        
        public void OnMouseDown()
        {
            Debug.Log("On Mouse Down");
        }

        public virtual void OnMouseUp()
        {
            Debug.Log("On Mouse Up");
            LineManager.Instance.HideLine();
        }

        public virtual void OnMouseDrag()
        {
            if (SelectingObjects.MouseOveredTile != null)
            {
                LineManager.Instance.DrawArc(GameGrid.Instance.WorldToCellPos(transform.position), GameGrid.Instance.WorldToCellPos(SelectingObjects.MouseOveredTile.transform.position));                
            }
        }
    }
}
