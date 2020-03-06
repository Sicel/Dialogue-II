using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// TODO: Add comments
public class DialogueTreeEditorWindow : EditorWindow
{
    public List<BaseNode> windows = new List<BaseNode>();

    private Vector2 mousePos;
    private Vector2 rightClickPos;

    private BaseNode selectedNode;

    private bool makeTransitionMode = false;
    private bool makeTransitionIn = false;
    private bool makeTransitionOut = false;

    private DialogueTree dialogueTree;
    private List<DialogueTreeElement> CurrentTree { get => dialogueTree.Dialogues; }

    private Rect box = new Rect(0, 0, 95, 35);
    private Rect Box
    {
        get
        {
            box.center = new Vector2(position.width / 2, position.height / 2);
            return box;
        }
    }

    public void ShowEditor(DialogueTree dialogueTree)
    {
        //this.interactable = interactable;
        DialogueTreeEditorWindow editor = GetWindow<DialogueTreeEditorWindow>();
        this.dialogueTree = dialogueTree;
        editor.titleContent.text = dialogueTree.gameObject.name + " DE";
        editor.Show();
        //DisplayTree(interactable.dialogueTree.serializedDialogueTree);
        DisplayTree();
    }

    private void OnEnable()
    {
        if (!dialogueTree)
        {
            GUI.Box(Box, "No Object Loaded");
            return;
        }
    }

    private void OnGUI()
    {
        if (!dialogueTree)
        {
            GUI.Box(Box, "No Object Loaded");
            return;
        }
        else if (windows.Count == 0)
        {
            GUI.Box(Box, "No Dialogue Yet");
        }

        Event e = Event.current;

        mousePos = e.mousePosition;

        if (e.button == 1 && !makeTransitionMode)
        {
            if (e.type == EventType.MouseDown)
            {
                bool clickedOnWindow = false;
                int selectedIndex = -1;

                for (int i = 0; i < windows.Count; i++)
                {
                    if (windows[i].windowRect.Contains(mousePos))
                    {
                        selectedIndex = i;
                        clickedOnWindow = true;
                        break;
                    }
                }

                if (!clickedOnWindow)
                {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Add Dialogue Node"), false, ContextCallback, "dialogue");
                    menu.AddItem(new GUIContent("Add Choice Node"), false, ContextCallback, "choice");

                    menu.ShowAsContext();
                    e.Use();
                }
                else
                {
                    rightClickPos = e.mousePosition;

                    GenericMenu menu = new GenericMenu();

                    // TODO: What items show up depend on which node is being clicked and where on the node it's being clicked

                    menu.AddItem(new GUIContent("Add As Input/Dialogue Node"), false, ContextCallback, "addInDialogue");
                    menu.AddItem(new GUIContent("Add As Input/Choice Node"), false, ContextCallback, "addInChoice");
                    menu.AddItem(new GUIContent("Add As Output/Dialogue Node"), false, ContextCallback, "addOutDialogue");
                    menu.AddItem(new GUIContent("Add As Output/Choice Node"), false, ContextCallback, "addOutChoice");
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Make Connection/Input"), false, ContextCallback, "makeTransitionIn");
                    menu.AddItem(new GUIContent("Make Connection/Output"), false, ContextCallback, "makeTransitionOut");
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Delete Node"), false, ContextCallback, "deleteNode");

                    menu.ShowAsContext();
                    e.Use();
                }
            }
        }
        else if (e.button == 0 && e.type == EventType.MouseDown && makeTransitionMode)
        {
            bool clickedOnWindow = false;
            int selectedIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectedIndex = i;
                    clickedOnWindow = true;
                    break;
                }
            }

            if (clickedOnWindow && !windows[selectedIndex].Equals(selectedNode))
            {
                if (makeTransitionIn)
                {
                    windows[selectedIndex].SetOutput(selectedNode, mousePos);
                    windows[windows.IndexOf(selectedNode)].SetInput(windows[selectedIndex], rightClickPos);
                    makeTransitionIn = false;
                }

                if (makeTransitionOut)
                {
                    windows[selectedIndex].SetInput(selectedNode, mousePos);
                    windows[windows.IndexOf(selectedNode)].SetOutput(windows[selectedIndex], rightClickPos);
                    makeTransitionOut = false;
                }
                makeTransitionMode = false;

                selectedNode = null;
            }

            if (!clickedOnWindow)
            {
                makeTransitionMode = false;
                makeTransitionIn = false;
                makeTransitionOut = false;
                selectedNode = null;
            }

            e.Use();
        }

        if (makeTransitionMode && selectedNode != null)
        {
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);

            DrawNodeCurve(selectedNode.windowRect, mouseRect);

            Repaint();
        }

        foreach (BaseNode n in windows)
        {
            n.DrawCurves();
        }

        BeginWindows();
        
        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTitle);
        }
        
        EndWindows();

        // TODO: Fix saving
        EditorUtility.SetDirty(dialogueTree);
    }

    private void DrawNodeWindow(int id)
    {
        windows[id].DrawWindow();
        GUI.DragWindow();
    }

    private void ContextCallback(object obj)
    {
        string clb = obj.ToString();
        bool clickedOnWindow = false;
        int selectedIndex = -1;
        switch (clb)
        {
            case "dialogue":
                DialogueNode dialogue = CreateInstance<DialogueNode>();
                dialogue.windowRect = new Rect(mousePos.x, mousePos.y, 300, 300);
                dialogue.index = windows.Count;

                windows.Add(dialogue);
                CurrentTree.Add(dialogue.ElementE);
                break;
            case "choice":
                ChoiceNode choice = CreateInstance<ChoiceNode>();
                choice.windowRect = new Rect(mousePos.x, mousePos.y, 300, 300);
                choice.index = windows.Count;

                windows.Add(choice);
                CurrentTree.Add(choice.ElementE);
                break;
            // TODO: Finish implementation
            case "addInDialogue":
                break;
            case "addInChoice":
                break;
            case "addOutDialogue":
                break;
            case "addOutChoice":
                break;
            case "makeTransitionIn":
                for (int i = 0; i < windows.Count; i++)
                {
                    if (windows[i].windowRect.Contains(mousePos))
                    {
                        selectedIndex = i;
                        clickedOnWindow = true;
                        break;
                    }
                }

                if (clickedOnWindow)
                {
                    selectedNode = windows[selectedIndex];
                    makeTransitionMode = true;
                    makeTransitionIn = true;
                }
                break;
            case "makeTransitionOut":
                for (int i = 0; i < windows.Count; i++)
                {
                    if (windows[i].windowRect.Contains(mousePos))
                    {
                        selectedIndex = i;
                        clickedOnWindow = true;
                        break;
                    }
                }

                if (clickedOnWindow)
                {
                    selectedNode = windows[selectedIndex];
                    makeTransitionMode = true;
                    makeTransitionOut = true;
                }
                break;
            case "deleteNode":
                clickedOnWindow = false;
                selectedIndex = -1;

                for (int i = 0; i < windows.Count; i++)
                {
                    if (windows[i].windowRect.Contains(mousePos))
                    {
                        selectedIndex = i;
                        clickedOnWindow = true;
                        break;
                    }
                }

                if (clickedOnWindow)
                {
                    BaseNode selNode = windows[selectedIndex];
                    CurrentTree.RemoveAt(selectedIndex);
                    windows.RemoveAt(selectedIndex);

                    foreach(BaseNode n in windows)
                    {
                        n.NodeDeleted(selNode);
                    }
                }
                break;
        }
    }

    internal static void DrawNodeCurve(Rect start, Rect end)
    {
        Vector2 startPos = new Vector2(start.x + (start.width / 2), start.y + (start.height / 2));
        Vector2 endPos = new Vector2(end.x + (end.width / 2), end.y + (end.height / 2));
        Vector2 midPos = (startPos + endPos) / 2;
        Vector2 startTan = startPos + Vector2.right * 50;
        Vector2 endTan = endPos + Vector2.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);

        //Handles.color = Color.blue;
        //Handles.DrawLine(startPos, midPos);
        //Handles.color = Color.red;
        //Handles.DrawLine(midPos, endPos);

        for (int i = 0; i < 3; i++)
        {
        //    Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }
        
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.red, null, 1);
        
    }

    void DisplayTree(List<IDialogueTreeElementInfo> sTree)
    {
        windows.Clear();

        foreach(IDialogueTreeElementInfo elementInfo in sTree)
        {
            DialogueNode newDialogue = null;
            ChoiceNode newChoice = null;

            if (elementInfo is DialogueElementInfo)
            {
                DialogueElementInfo dialogue = (DialogueElementInfo)elementInfo;

                newDialogue = CreateInstance<DialogueNode>();

                newDialogue.windowRect = new Rect(dialogue.WindowRect.x, dialogue.WindowRect.y, 300, 300);
                newDialogue.index = dialogue.Index;
                if (dialogue.HasInputs)
                    newDialogue.inputRects = dialogue.InputRects;
                if (dialogue.HasOutputs)
                    newDialogue.outputRects = dialogue.OutputRects;
                newDialogue.sentences = dialogue.Sentences;

                windows.Add(newDialogue);
            }
            else if (elementInfo is ChoiceElementInfo)
            {
                ChoiceElementInfo choice = (ChoiceElementInfo)elementInfo;

                newChoice = CreateInstance<ChoiceNode>();
                newChoice.windowRect = new Rect(choice.WindowRect.x, choice.WindowRect.y, 300, 300);
                newChoice.index = choice.Index;
                if (choice.HasInputs)
                    newChoice.inputRects = choice.InputRects;
                if (choice.HasOutputs)
                    newChoice.outputRects = choice.OutputRects;
                newChoice.prompt = choice.Prompt;
                newChoice.numChoices = choice.NumChoices;
                newChoice.choices = choice.Choices;
                newChoice.choiceRects = choice.ChoiceRects;

                windows.Add(newChoice);
            }
        }

        for (int i = 0; i < windows.Count; i++)
        {
            IDialogueTreeElementInfo info = sTree[i];

            // Sets inputs
            for (int j = 0; j < info.InputCount; j++)
            {
                if (!windows[i].inputs.Contains(windows[info.InputIndexes[j]]))
                {
                    windows[i].inputs.Add(windows[info.InputIndexes[j]]);
                }
            }

            // Sets outputs
            for (int j = 0; j < info.OutputCount; j++)
            {
                if (!windows[i].outputs.Contains(windows[info.OutputIndexes[j]]))
                {
                    windows[i].outputs.Add(windows[info.OutputIndexes[j]]);
                }
            }

            if (windows[i] is ChoiceNode)
            {
                ChoiceElementInfo cInfo = (ChoiceElementInfo)info;

                // Copy of node to edit
                ChoiceNode choice = windows[i] as ChoiceNode;

                // sets outputs with corresponding choice
                for (int j = 0; j < cInfo.ChoiceDialogueKeys.Count; j++)
                {
                    choice.choiceNodePair.Add(j, windows[cInfo.ChoiceDialogueValues[j]]);
                }

                // Rewrites previous node
                windows[i] = choice;
            }
        }
    }

    void DisplayTree()
    {
        // TODO: Fix connections on open
        windows.Clear();

        foreach (DialogueTreeElement element in CurrentTree)
        {
            DialogueNode newDialogue = null;
            ChoiceNode newChoice = null;

            if (element is DialogueElement)
            {
                DialogueElementInfo dialogue = (DialogueElementInfo)element.ElementInfo;

                newDialogue = CreateInstance<DialogueNode>();
                newDialogue.Init(element);

                windows.Add(newDialogue);
            }
            else if (element is ChoiceElement)
            {
                ChoiceElementInfo choice = (ChoiceElementInfo)element.ElementInfo;

                newChoice = CreateInstance<ChoiceNode>();
                newChoice.Init(element);

                windows.Add(newChoice);
            }
        }

        for (int i = 0; i < windows.Count; i++)
        {
            IDialogueTreeElementInfo info = windows[i].ElementE.ElementInfo;

            // Sets inputs
            for (int j = 0; j < info.InputCount; j++)
            {
                if (!windows[i].inputs.Contains(windows[info.InputIndexes[j]]))
                {
                    windows[i].inputs.Add(windows[info.InputIndexes[j]]);
                }
            }

            // Sets outputs
            for (int j = 0; j < info.OutputCount; j++)
            {
                if (!windows[i].outputs.Contains(windows[info.OutputIndexes[j]]))
                {
                    windows[i].outputs.Add(windows[info.OutputIndexes[j]]);
                }
            }

            if (windows[i] is ChoiceNode)
            {
                ChoiceElementInfo cInfo = (ChoiceElementInfo)info;

                // Copy of node to edit
                ChoiceNode choice = windows[i] as ChoiceNode;

                // sets outputs with corresponding choice
                for (int j = 0; j < cInfo.ChoiceDialogueKeys.Count; j++)
                {
                    choice.choiceNodePair.Add(j, windows[cInfo.ChoiceDialogueValues[j]]);
                }

                // Rewrites previous node
                windows[i] = choice;
            }
        }
    }
}
