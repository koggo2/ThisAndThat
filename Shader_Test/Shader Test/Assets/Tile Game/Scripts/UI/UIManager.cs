using System.Collections.Generic;
using TheTile.Game;
using TheTile.Game.Unit;
using TheTile.UI.Interface;
using UnityEngine;

namespace TheTile.UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Canvas _canvas;
        private Camera _mainCamera;
        private RectTransform _canvasRect;

        private Dictionary<int, List<UIBaseComponent>> _objectUIs = new Dictionary<int, List<UIBaseComponent>>();
        private List<IAutoUpdating> _autoUpdatingUI = new List<IAutoUpdating>();
        private List<IAutoPositioning> _autoPositioningUI = new List<IAutoPositioning>();

        private void Awake()
        {
            _mainCamera = Camera.main;
            if(_canvas != null)
                _canvasRect = _canvas.GetComponent<RectTransform>();
        }

        public void RegisterBasementInfoUI(BaseBasement basement)
        {
            var prefab = Resources.Load<GameObject>($"UI/{ConstData.UI_BasementInfoName}");
            if (prefab != null)
            {
                var instance = Instantiate(prefab);
                instance.transform.parent = _canvas.transform;
                instance.transform.localScale = Vector3.one;
                instance.transform.localRotation = Quaternion.identity;
                    
                var basementInfo = instance.GetComponent<UIBasementInfo>();
                basementInfo.Init(basement);

                var objectInstanceId = basement.gameObject.GetInstanceID();
                if (!_objectUIs.ContainsKey(objectInstanceId))
                {
                    _objectUIs.Add(objectInstanceId, new List<UIBaseComponent>());
                }
                _objectUIs[objectInstanceId].Add(basementInfo);
                    
                _autoUpdatingUI.Add(basementInfo);
                _autoPositioningUI.Add(basementInfo);
            }
        }

        public void RegisterUnitInfoUI(BaseUnit unit)
        {
            var prefab = Resources.Load<GameObject>($"UI/{ConstData.UI_UnitInfoName}");
            if (prefab != null)
            {
                var instance = Instantiate(prefab);
                instance.transform.SetParent(_canvas.transform);
                instance.transform.localScale = Vector3.one;
                instance.transform.localRotation = Quaternion.identity;
                    
                var unitInfo = instance.GetComponent<UIUnitInfo>();
                unitInfo.Init(unit);

                var objectInstanceId = unit.gameObject.GetInstanceID();
                if (!_objectUIs.ContainsKey(objectInstanceId))
                {
                    _objectUIs.Add(objectInstanceId, new List<UIBaseComponent>());
                }
                _objectUIs[objectInstanceId].Add(unitInfo);
                    
                _autoUpdatingUI.Add(unitInfo);
                _autoPositioningUI.Add(unitInfo);
            }
        }

        public void UnregisterUI(int objectInstanceId)
        {
            if (!_objectUIs.ContainsKey(objectInstanceId)) return;
            
            _objectUIs[objectInstanceId].ForEach(uiComponent => DestroyImmediate(uiComponent.gameObject));
            _objectUIs[objectInstanceId].Clear();
            _objectUIs.Remove(objectInstanceId);
        }
        
        private void Update()
        {
            _autoUpdatingUI.ForEach(i => i.UpdateUI());
            _autoPositioningUI.ForEach(i => i.UpdatePosition(_mainCamera, _canvasRect));
        }
    }
}
