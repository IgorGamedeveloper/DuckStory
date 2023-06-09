using UnityEngine;
using UnityEditor;

namespace IMG.Grid
{
    [CustomEditor(typeof(SceneGrid))]
    public class SceneGridEditor : Editor
    {
        private SceneGrid _target;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(25f);
            if (GUILayout.Button("Создать сетку!"))
            {
                _target = target as SceneGrid;
                _target.InstantiateGrid();
            }

            GUILayout.Space(10f);
            if (GUILayout.Button("Удалить сетку!"))
            {
                _target = target as SceneGrid;
                _target.DestroyGrid();
            }
        }
    }
}
