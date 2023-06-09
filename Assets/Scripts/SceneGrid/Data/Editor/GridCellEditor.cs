using UnityEngine;
using UnityEditor;

namespace IMG.Grid
{
    [CustomEditor(typeof(GridCell))]
    [CanEditMultipleObjects]
    public class GridCellEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            GUILayout.Space(25f);
            if (GUILayout.Button("������� �����������!"))
            {
                Transform[] selectionTransforms = Selection.transforms;

                for (int i = 0; i < selectionTransforms.Length; i++)
                {
                    selectionTransforms[i].GetComponent<GridCell>().SelectRandomObstacleModel();
                }
            }
            
            GUILayout.Space(10f);
            if (GUILayout.Button("������ �����������!"))
            {
                Transform[] selectionTransforms = Selection.transforms;

                for (int i = 0; i < selectionTransforms.Length; i++)
                {
                    selectionTransforms[i].GetComponent<GridCell>().SelectRandomCellModel();
                }
            }
        }
    }
}
