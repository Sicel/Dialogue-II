using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DialogueTree))]
public class DialogueTreePropertyDrawer : PropertyDrawer
{
    List<IDialogueTreeElementInfo> tree;
    Interactable interactable;
    bool showTree = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        interactable = (Interactable)property.serializedObject.targetObject;
        tree = interactable.dialogueTree.serializedDialogueTree;
        //base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty index = property.FindPropertyRelative("index");

        showTree = EditorGUI.Foldout(position, showTree, label);
        if (showTree)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(index);
            for (int i = 0; i < tree.Count; i++)
            {
                EditorGUILayout.LabelField(string.Format("Element {0}:", i));
                EditorGUI.indentLevel++;
                ShowElementInfo(tree[i]);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    void ShowElementInfo(IDialogueTreeElementInfo elementInfo)
    {
        EditorGUILayout.LabelField("Index:", elementInfo.Index.ToString());
        EditorGUILayout.LabelField("Inputs:", elementInfo.InputCount.ToString());
        EditorGUILayout.LabelField("Outputs:", elementInfo.OutputCount.ToString());
        if (elementInfo is DialogueElementInfo)
            ShowElementInfo((DialogueElementInfo)elementInfo);
        else if (elementInfo is ChoiceElementInfo)
            ShowElementInfo((ChoiceElementInfo)elementInfo);
    }

    void ShowElementInfo(DialogueElementInfo elementInfo)
    {
        EditorGUILayout.LabelField("Num Sentences:", elementInfo.Sentences.Count.ToString());
    }

    void ShowElementInfo(ChoiceElementInfo elementInfo)
    {
        EditorGUILayout.LabelField("Prompt:", elementInfo.Prompt);
        EditorGUILayout.LabelField("Num Choices:", elementInfo.NumChoices.ToString());
        EditorGUILayout.LabelField("Num Connections:", elementInfo.ChoiceDialogueKeys.Count.ToString());
    }
}
