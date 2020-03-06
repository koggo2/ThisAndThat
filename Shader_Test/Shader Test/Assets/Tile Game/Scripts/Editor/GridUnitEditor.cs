using TheTile.Game;
using UnityEditor;
using UnityEngine;

namespace TheTile.Editor
{
    [CustomEditor(typeof(GridUnit), true)]
    [CanEditMultipleObjects]
    public class GridUnitEditor : UnityEditor.Editor
    {
        private GridUnit _gridUnit;

        public void OnEnable()
        {
            _gridUnit = target as GridUnit;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Label("Tile");
            if (GUILayout.Button("Clear Tile"))
            {
                foreach (var go in Selection.gameObjects)
                {
                    var gridUnit = go.GetComponent<GridUnit>();
                    if (gridUnit != null)
                    {
                        gridUnit.ClearTile();
                    }
                }
            }
            
            if (GUILayout.Button("Create None Tile"))
            {
                foreach (var go in Selection.gameObjects)
                {
                    var gridUnit = go.GetComponent<GridUnit>();
                    if (gridUnit != null)
                    {
                        gridUnit.CreateTile(ConstData.Object_NoneTileName);
                    }
                }
                
            }

            if (GUILayout.Button("Create Empty Tile"))
            {
                foreach (var go in Selection.gameObjects)
                {
                    var gridUnit = go.GetComponent<GridUnit>();
                    if (gridUnit != null)
                    {
                        gridUnit.CreateTile(ConstData.Object_EmptyTileName);
                    }
                }
            }

            GUILayout.Space(10f);
            GUILayout.Label("Basement");
            if (GUILayout.Button("Create House"))
            {
                foreach (var go in Selection.gameObjects)
                {
                    var gridUnit = go.GetComponent<GridUnit>();
                    if (gridUnit != null)
                    {
                        gridUnit.CreateBasement(ConstData.Object_HouseBasementName);
                    }
                }
            }
            
            GUILayout.Space(10f);
            GUILayout.Label("Team");
            if (GUILayout.Button("Set Team A"))
            {
                foreach (var go in Selection.gameObjects)
                {
                    var gridUnit = go.GetComponent<GridUnit>();
                    if (gridUnit != null)
                    {
                        gridUnit.SetTeam(BaseObject.TeamEnum.A);
                    }
                }
            }
            
            if (GUILayout.Button("Set Team B"))
            {
                foreach (var go in Selection.gameObjects)
                {
                    var gridUnit = go.GetComponent<GridUnit>();
                    if (gridUnit != null)
                    {
                        gridUnit.SetTeam(BaseObject.TeamEnum.B);
                    }
                }
            }
        }
    }
}