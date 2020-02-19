using TheTile.Game;
using TheTile.UI.Interface;
using TMPro;
using UnityEngine;

namespace TheTile.UI
{
    public class UIBasementInfo : UIBaseComponent, IAutoUpdating, IAutoPositioning
    {
        [SerializeField] private int _offsetX;
        [SerializeField] private int _offsetY;
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private BaseBasement _targetBaseObject;

        public void Init(BaseBasement basement)
        {
            _targetBaseObject = basement;
        }
        
        public void UpdateUI()
        {
            if (_textMeshPro != null && _targetBaseObject != null)
                _textMeshPro.text = $"{_targetBaseObject.Hp} / {_targetBaseObject.ConstructionValue}";
        }

        public void UpdatePosition(Camera camera, RectTransform canvasRect)
        {
            if (_targetBaseObject != null)
            {
                var viewportPos = camera.WorldToViewportPoint(_targetBaseObject.transform.position);

                var pos = new Vector2((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f) + _offsetX,
                    (viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f) + _offsetY);
                transform.localPosition = pos;
            }
        }
    }
}
