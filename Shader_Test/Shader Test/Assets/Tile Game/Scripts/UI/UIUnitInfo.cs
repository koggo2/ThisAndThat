using TheTile.Game;
using TheTile.UI.Interface;
using TMPro;
using UnityEngine;

namespace TheTile.UI
{
    public class UIUnitInfo : UITargetComponent<BaseUnit>, IAutoUpdating, IAutoPositioning
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        
        public void UpdateUI()
        {
            if (_textMeshPro != null && Target != null)
                _textMeshPro.text = $"{Target.Hp}";
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
