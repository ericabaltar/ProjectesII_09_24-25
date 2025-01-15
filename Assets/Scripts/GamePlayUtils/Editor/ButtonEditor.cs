using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ButtonGameplay))]
public class ButtonEditor : Editor
{
    SerializedProperty door;
    SerializedProperty buttonSpriteRenderer;
    SerializedProperty pressedSprite;
    SerializedProperty defaultSprite;

    private void OnEnable()
    {
        door = serializedObject.FindProperty("door");
        buttonSpriteRenderer = serializedObject.FindProperty("buttonSpriteRenderer");
        pressedSprite = serializedObject.FindProperty("pressedSprite");
        defaultSprite = serializedObject.FindProperty("defaultSprite");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(door, new GUIContent("Door"));
        EditorGUILayout.PropertyField(buttonSpriteRenderer, new GUIContent("Button Sprite Renderer"));
        EditorGUILayout.PropertyField(pressedSprite, new GUIContent("Pressed Sprite"));
        EditorGUILayout.PropertyField(defaultSprite, new GUIContent("Default Sprite"));

        serializedObject.ApplyModifiedProperties();
    }
}

