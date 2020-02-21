using TheTile.Game.Unit;
using TheTile.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheTile.Game
{
    public class BaseTile : BaseObject
    {
        public Vector3Int CellPos;

        public Transform Pivot => _pivot;
        [SerializeField] private Transform _pivot;

        private GameObject _selectionUI = null;
        
        [ExecuteInEditMode]
        private void Awake()
        {
            _pivot.localPosition = new Vector3(0f, Random.Range(-1f, 1f), 0f);
        }
        
        private void OnMouseEnter()
        {
            if (_selectionUI != null)
            {
                DestroyImmediate(_selectionUI);                
            }
            
            var selectionUIPrefab = Resources.Load<GameObject>("Tile Selection");
            var instance = Instantiate(selectionUIPrefab);
            instance.transform.parent = _pivot;
            instance.transform.localPosition = new Vector3(0f, 1f, 0f);
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;

            _selectionUI = instance;

            SelectingObjects.MouseOveredCellPos = GameGrid.Instance.WorldToCellPos(transform.position);
        }

        private void OnMouseDown()
        {
            var thisTileData = GameGrid.Instance.GetUnderTileData(this);
            if(thisTileData.Basement != null)
                SelectingObjects.SelectedBasement = thisTileData.Basement;
        }

        private void OnMouseDrag()
        {
            var mouseOveredTileCellPos = SelectingObjects.MouseOveredCellPos;
            var cellPos = GameGrid.Instance.WorldToCellPos(transform.position);

            if (mouseOveredTileCellPos != cellPos)
            {
                LineManager.Instance.DrawArc(cellPos, mouseOveredTileCellPos);                    
            }
        }
        
        private void OnMouseUp()
        {
            if(SelectingObjects.SelectedBasement != null)
                GameController.Instance.MarchNBuild(GameGrid.Instance.WorldToCellPos(transform.position), SelectingObjects.MouseOveredCellPos);
            
            SelectingObjects.SelectedBasement = null;
            LineManager.Instance.HideLine();
        }

        private void OnMouseExit()
        {
            if (_selectionUI != null)
            {
                DestroyImmediate(_selectionUI);                
            }
        }
    }
}
