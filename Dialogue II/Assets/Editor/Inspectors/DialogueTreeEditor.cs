using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueTree))]
public class DialogueTreeEditor : Editor
{
    DialogueTree interactable;

    private void OnEnable()
    {
        interactable = (DialogueTree)target;
    }

    public override void OnInspectorGUI()
    {
        //EditorGUILayout.LabelField(interactable.dialogueTree.dialogues.Count.ToString());

        base.OnInspectorGUI();

        EditorGUILayout.LabelField(interactable.dialogues.Count.ToString());

        if (GUILayout.Button("Open Dialogue Editor"))
        {
            DialogueTreeEditorWindow treeEditorWindow = CreateInstance<DialogueTreeEditorWindow>();
            treeEditorWindow.ShowEditor(interactable);
            GUIUtility.ExitGUI();
        }
    }
}
