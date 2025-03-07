using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(PersistentPrefabSpawner))]
public class PersistentPrefabSpawnerEditor : Editor
{
    private PersistentPrefabSpawner script;

    private void OnEnable()
    {
        script = (PersistentPrefabSpawner)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw the default fields

        if (script.prefabStorage == null)
        {
            EditorGUILayout.HelpBox("Assign a PrefabStorage asset!", MessageType.Warning);
            return;
        }

        EditorGUILayout.LabelField("Select a Prefab", EditorStyles.boldLabel);

        float windowWidth = EditorGUIUtility.currentViewWidth; // Get the inspector width
        int maxColumns = Mathf.FloorToInt(windowWidth / 80); // Max columns based on inspector width
        int currentColumn = 0; // Track columns
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        foreach (var entry in script.prefabStorage.prefabs)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(entry.prefabPath);
            Texture2D preview = AssetPreview.GetAssetPreview(prefab);

            EditorGUILayout.BeginVertical(GUILayout.Width(80)); // Keep name centered above image
            EditorGUILayout.LabelField(entry.name, EditorStyles.centeredGreyMiniLabel, GUILayout.Width(80));

            if (GUILayout.Button(preview, GUILayout.Width(64), GUILayout.Height(64)))
            {
                script.SetSelectedPrefab(entry.prefabPath);
            }
            EditorGUILayout.EndVertical();

            currentColumn++;

            // If max columns reached, start a new row
            if (currentColumn >= maxColumns)
            {
                EditorGUILayout.EndHorizontal(); // Close current row
                EditorGUILayout.BeginHorizontal(); // Start new row
                currentColumn = 0;
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Spawn Selected Prefab"))
        {
            script.SpawnPrefab();
        }
    }
}
