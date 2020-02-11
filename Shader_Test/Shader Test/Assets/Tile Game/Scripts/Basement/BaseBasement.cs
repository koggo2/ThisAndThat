using System;
using TheTile.Util;
using UnityEngine;

namespace TheTile.Game
{
    public class BaseBasement : BaseObject
    {
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
