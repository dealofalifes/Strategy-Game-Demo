using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(BuildingElementView))]
public class BuildingElementEditor : Editor
{
    //private Vector2Int _GridSize;
    //private Vector2Int _CellSize;
    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector();

    //    if (Application.isPlaying)
    //    {
    //        GUILayout.Space(25);
    //        GUILayout.Label("Cannot be edited in runtime!", EditorStyles.boldLabel);
    //        return;
    //    }

    //    GUILayout.Space(25);

    //    GUILayout.Label("Building Size", EditorStyles.boldLabel);
    //    GUILayout.Label("To create prefab", EditorStyles.miniLabel);
    //    _GridSize = EditorGUILayout.Vector2IntField("Building Size:",
    //        new(Mathf.Clamp(_GridSize.x, 1, 10), Mathf.Clamp(_GridSize.y, 1, 10)));

    //    GUILayout.Space(15);

    //    GUILayout.Label("Cell Size", EditorStyles.boldLabel);
    //    _CellSize = EditorGUILayout.Vector2IntField("Cell Size:",
    //        new(Mathf.Clamp(_CellSize.x, 32, 128), Mathf.Clamp(_CellSize.y, 32, 128)));

    //    GridSystemController gridSystemController = (GridSystemController)target;
    //    if (GUILayout.Button("Generate Grid Map"))
    //    {
    //        gridSystemController.CreateGridArea(_GridSize, _CellSize);
    //        EditorUtility.SetDirty(gridSystemController);

    //    }

    //    if (GUILayout.Button("Clear Grid Map"))
    //    {
    //        gridSystemController.CreateGridArea(Vector2Int.zero, Constants._OriginalCellSize);
    //        EditorUtility.SetDirty(gridSystemController);
    //    }
    //}
}
