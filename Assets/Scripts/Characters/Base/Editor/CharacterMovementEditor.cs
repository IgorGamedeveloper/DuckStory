using UnityEngine;
using IMG.Character;
using IMG.Grid;
using UnityEditor;

[CustomEditor(typeof(CharacterMovement))]
public abstract class CharacterMovementEditor : Editor
{
    public const float Y_OFFSET = 1.9f;
    public const float ROTATE_ANGLE = 90.0f;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(25f);

        if (GUILayout.Button("Притянуть к сетке!"))
        {
            CharacterMovement currentMovement = target as CharacterMovement;
            Transform transform = currentMovement.transform;
            SceneGrid grid = FindObjectOfType<SceneGrid>();
            grid.GetXY(transform.position, out int xCell, out int yCell);
            Vector3 targetPosition = FindObjectOfType<SceneGrid>().Data[xCell, yCell].transform.position;
            targetPosition.y = Y_OFFSET;
            transform.position = targetPosition;
        }
        GUILayout.Space(25f);

        if (GUILayout.Button("Повернуть влево!"))
        {
            CharacterMovement currentMovement = target as CharacterMovement;
            Transform transform = currentMovement.transform;
            Vector3 currentRotate = transform.eulerAngles;
            currentRotate.y -= ROTATE_ANGLE;
            transform.rotation = Quaternion.Euler(currentRotate);
        }
        GUILayout.Space(10f);

        if (GUILayout.Button("Повернуть вправо!"))
        {
            CharacterMovement currentMovement = target as CharacterMovement;
            Transform transform = currentMovement.transform;
            Vector3 currentRotate = transform.eulerAngles;
            currentRotate.y += ROTATE_ANGLE;
            transform.rotation = Quaternion.Euler(currentRotate);
        }
    }
}
