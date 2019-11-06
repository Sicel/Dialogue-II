using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DialogueTree))]
public class DialogueTreePropertyDrawer : PropertyDrawer
{
    bool showTree = false;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty tree = property.FindPropertyRelative("serializedDialogueTree");
        SerializedProperty index = property.FindPropertyRelative("index");

        showTree = EditorGUI.Foldout(position, showTree, label);
        if (showTree)
        {
            //for (int i = 0; i < tree.arraySize; i++)
            //{
            //    EditorGUILayout.PropertyField(tree.GetArrayElementAtIndex(i));
            //}
            EditorGUILayout.PropertyField(index);
        }

        EditorGUI.EndProperty();
    }
}
