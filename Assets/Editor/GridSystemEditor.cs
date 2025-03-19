using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GridSystemController))]
public class GridSystemEditor : Editor
{
    private Vector2Int _GridSize;
    private Vector2Int _CellSize;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (Application.isPlaying)
        {
            GUILayout.Space(25);
            GUILayout.Label("Cannot be edited in runtime!", EditorStyles.boldLabel);
            return;
        }

        GUILayout.Space(25);

        GUILayout.Label("Grid Size", EditorStyles.boldLabel);
        _GridSize = EditorGUILayout.Vector2IntField("Grid Size:",
            new(Mathf.Clamp(_GridSize.x, 5, 50), Mathf.Clamp(_GridSize.y, 5, 50)));

        GUILayout.Space(15);

        GUILayout.Label("Cell Size", EditorStyles.boldLabel);
        _CellSize = EditorGUILayout.Vector2IntField("Cell Size:",
            new(Mathf.Clamp(_CellSize.x, 32, 128), Mathf.Clamp(_CellSize.y, 32, 128)));

        GridSystemController gridSystemController = (GridSystemController)target;
        if (GUILayout.Button("Set Grid Map Size"))
        {
            gridSystemController.CreateGridArea(_GridSize, _CellSize);
            EditorUtility.SetDirty(gridSystemController);
            Debug.Log("Grid map saved with size: " + _GridSize.x + "/" + _GridSize.y + "\nCell Size: " + _CellSize.x + "/" + _CellSize.y);
        }
        
        if (GUILayout.Button("Reset Grid Map Size"))
        {
            gridSystemController.CreateGridArea(Vector2Int.zero, Constants._OriginalCellSize);
            EditorUtility.SetDirty(gridSystemController);
            Debug.Log("Grid map saved with size: " + 25 + "/" + 25 + "\nCell Size: " + 64 + "/" + 64);
        }
    }
}
