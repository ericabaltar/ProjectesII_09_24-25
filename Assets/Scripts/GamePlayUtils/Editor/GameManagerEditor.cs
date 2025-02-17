using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get reference to the GameManager script
        GameManager gameManager = (GameManager)target;

        // Draw default inspector fields
        DrawDefaultInspector();

        // Show the GameObject field only if typeOfCenter == TypeOfCenter.GameObject
        if (gameManager.GetTypeOfCenter() == GameManager.TypeOfCenter.GameObject)
        {
            SerializedProperty centerGameObjectProp = serializedObject.FindProperty("centerGameObject");
            EditorGUILayout.PropertyField(centerGameObjectProp, new GUIContent("Center GameObject"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
