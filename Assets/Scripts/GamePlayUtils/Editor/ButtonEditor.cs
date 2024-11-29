using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[CustomEditor(typeof(ButtonGameplay))]
public class ButtonEditor : Editor
{
    #region SerializeProperty

    SerializedProperty buttonType;
    SerializedProperty objectToEdit;
    SerializedProperty timer;
    SerializedProperty timerReset;
    SerializedProperty timeToMove;

    #endregion

    private void OnEnable()
    {
        buttonType = serializedObject.FindProperty("buttonTypes");
        objectToEdit = serializedObject.FindProperty("objectToEdit");
        timer = serializedObject.FindProperty("timer");
        timerReset = serializedObject.FindProperty("timerReset");
        timeToMove = serializedObject.FindProperty("timeToMove");

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(buttonType);
        if(buttonType.enumValueIndex != 0)
        {
            EditorGUILayout.PropertyField(objectToEdit);
        }
        
        if(buttonType.enumValueIndex == 2)
        {
            EditorGUILayout.PropertyField(timeToMove);
        }

        if (buttonType.enumValueIndex == 3 )
        {
            EditorGUILayout.PropertyField (timer);
            EditorGUILayout.PropertyField (timerReset);

        }


        
        serializedObject.ApplyModifiedProperties();
    }
}
