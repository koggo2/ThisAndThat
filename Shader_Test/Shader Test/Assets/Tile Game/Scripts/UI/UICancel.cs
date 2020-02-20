using TheTile.Game;
using TheTile.UI.Interface;
using UnityEngine;

namespace TheTile.UI
{
    public class UICancel : UITargetComponent<BaseBasement>, IAutoPositioning
    {
        public void OnClickCancel()
        {
            var cellPos = GameGrid.Instance.WorldToCellPos(Target.transform.position);
            GameController.Instance.CancelConstruction(cellPos);
        }

        public void UpdatePosition(Camera camera, RectTransform canvasRect)
        {
            if (Target != null)
            {
                var viewportPos = camera.WorldToViewportPoint(Target.transform.position);

                var pos = new Vector2((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f) + _offsetX,
                    (viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f) + _offsetY);
                transform.localPosition = pos;
            }
        }
    }
}
