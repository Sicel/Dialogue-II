using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    Interactable interactable;

    private void OnEnable()
    {
        interactable = (Interactable)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField(interactable.dialogueTree.dialogues.Count.ToString());

        base.OnInspectorGUI();

        if (GUILayout.Button("Open Dialogue Editor"))
        {
            DialogueTreeEditorWindow treeEditorWindow = CreateInstance<DialogueTreeEditorWindow>();
            treeEditorWindow.ShowEditor(interactable);
            GUIUtility.ExitGUI();
        }
    }
}
