using System;
using UnityEditor;
using UnityEngine;

namespace TheTile.Editor
{
    public class GameMapEditorWindow : EditorWindow
    {
        private bool _changeTileToNone = false;
        private bool _changeTileToEmpty = false;
        
        [MenuItem ("The Tile/Editor Window")]
        static void Init () {
            // Get existing open window or if none, make a new one:
            var window = (GameMapEditorWindow)EditorWindow.GetWindow (typeof (GameMapEditorWindow));
            window.Show();
        }

        private void OnFocus()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
            SceneView.duringSceneGui += this.OnSceneGUI;
        }

        private void OnDestroy()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }

        void OnGUI () 
        {   
            GUILayout.Label ("Base Create Map", EditorStyles.boldLabel);
            if (GUILayout.Button("Square Tile Map 10 x 10"))
            {
                var tileMap = FindObjectOfType<GameHeart>();
                tileMap.GenerateTileMap();
            }
		
            GUILayout.Label ("Change Tile", EditorStyles.boldLabel);
            _changeTileToNone = BasicToggleButton("None Tile", _changeTileToNone, ResetChangeTileValues);
            _changeTileToEmpty = BasicToggleButton("Empty Tile", _changeTileToEmpty, ResetChangeTileValues);
            
            GUILayout.Label ("Set Basement", EditorStyles.boldLabel);
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            Vector2 mousePosition = Event.current.mousePosition;
            Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay( new Vector3( mousePosition.x, Screen.height - mousePosition.y, 0 ) );
 
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f))
            {
                Debug.Log($"Hit, go : {hit.collider.gameObject}, pos : {hit.point}");
                Handles.DrawDottedLine(hit.point - new Vector3(1f, 0f, 0f), hit.point + Vector3.one, 4f);
            }
            
            // if (Event.current.type == EventType.MouseDown)
            // {
            //     
            // }   
        }

        private bool BasicToggleButton(string label, bool value, Action resetToggleGroupAction)
        {
            var rtn = false;
            GUILayout.BeginHorizontal();
            {
                rtn = GUILayout.Toggle(value, label, "button");
                if (rtn)
                {
                    resetToggleGroupAction?.Invoke();
                    if (GUILayout.Button("X"))
                        rtn = false;
                }
            }
            GUILayout.EndHorizontal();

            return rtn;
        }

        private void ResetChangeTileValues()
        {
            _changeTileToNone = false;
            _changeTileToEmpty = false;
        }
    }
}
