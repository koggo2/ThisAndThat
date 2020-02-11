
using TheTile.Game;
using UnityEditor;
using UnityEngine;

namespace TheTile.Editor
{
    [CustomEditor(typeof(BaseTile), true)]
    public class BaseTileEditor : UnityEditor.Editor
    {
        private BaseTile _tile;

        public void OnEnable()
        {
            _tile = target as BaseTile;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Create House"))
            {
                var basementPrefab = Resources.Load<GameObject>("House Basement");
                var basementInstance = Instantiate(basementPrefab);
                basementInstance.transform.SetParent(_tile.Pivot);
                basementInstance.transform.localPosition = Vector3.zero;
            
                var basement = basementInstance.GetComponent<BaseBasement>();
                basement.Team = _tile.Team;
            }
        }
    }
}
