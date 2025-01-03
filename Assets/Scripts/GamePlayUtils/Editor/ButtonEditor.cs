using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[CustomEditor(typeof(ButtonGameplay))]
public class ButtonEditor : Editor
{
    #region SerializeProperty
    SerializedProperty door; 
    #endregion

    private void OnEnable()
    {
        door = serializedObject.FindProperty("door");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(door, new GUIContent("Door"));

        serializedObject.ApplyModifiedProperties();
    }
}

