
using TheTile.Game;
using UnityEditor;

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
        }
    }
}
