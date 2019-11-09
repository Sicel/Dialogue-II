using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ChoiceNode : BaseNode
{
    public int numChoices;

    public string prompt;
    public List<string> choices;
    public List<Rect> choiceRects;
    public Dictionary<int, BaseNode> choiceNodePair;

    public ChoiceElementInfo Element
    {
        get { return (ChoiceElementInfo)treeElement.ElementInfo; }
        set { treeElement.ElementInfo = value; }
    }

    public int NumChoices { get => choices.Count; }

    public ChoiceNode()
    {
        windowTitle = "Choice Node";

        inputs = new List<BaseNode>();
        inputRects = new List<Rect>();

        outputs = new List<BaseNode>();
        outputRects = new List<Rect>();

        numChoices = 2;
        choices = new List<string>();
        choiceRects = new List<Rect>();
        choiceNodePair = new Dictionary<int, BaseNode>();

        Element = new ChoiceElementInfo()
        {
            WindowRect = windowRect,
            Index = index,
            InputIndexes = new List<int>(),
            InputRects = inputRects,
            OutputIndexes = new List<int>(),
            OutputRects = outputRects,
            Choices = choices,
            ChoiceRects = choiceRects,
            ChoiceDialogueKeys = new List<int>(),
            ChoiceDialogueValues = new List<int>()
        };
    }

    public override void Init(DialogueTreeElement element)
    {
        base.Init(element);

        ChoiceElement choiceElement = (ChoiceElement)element;
        Element = (ChoiceElementInfo)treeElement.ElementInfo;

        choices = choiceElement.choices;

        prompt = Element.Prompt;
        numChoices = choices.Count;
        choiceRects = Element.ChoiceRects;
    }

    public override void DrawWindow()
    {
        base.DrawWindow();

        Event e = Event.current;

        EditorGUILayout.LabelField("Prompt:");
        prompt = EditorGUILayout.TextArea(prompt);

        numChoices = EditorGUILayout.IntField("Number of Choices", numChoices);
        if (numChoices < 0)
        {
            numChoices = 0;
        }

        EditorGUILayout.LabelField("Choices:");
        EditorGUILayout.Space();

        int difference = Mathf.Abs(numChoices - choices.Count);
        if (choices.Count < numChoices)
        {
            for (int i = 0; i < difference; i++)
            {
                choices.Add("");
                choiceRects.Add(new Rect());
            }
        }
        else if (choices.Count > numChoices)
        {
            if (numChoices != 0)
            {
                choices.RemoveRange(numChoices - 1, difference);
                choiceRects.RemoveRange(numChoices - 1, difference);
                for (int i = numChoices; i < difference; i++)
                {
                    if (choiceNodePair.ContainsKey(i))
                    {
                        choiceNodePair.Remove(i);
                        Element.ChoiceDialogueKeys.RemoveAt(i);
                        Element.ChoiceDialogueValues.RemoveAt(i);
                    }
                }
            }
            else
            {
                choices.Clear();
                choiceRects.Clear();
                choiceNodePair.Clear();
                Element.ChoiceDialogueKeys.Clear();
                Element.ChoiceDialogueValues.Clear();
            }
        }

        for (int i = 0; i < inputRects.Count; i++)
        {
            if (e.type == EventType.Repaint)
            {
                inputRects[i] = GUILayoutUtility.GetLastRect();
            }
        }

        for (int i = 0; i < outputRects.Count; i++)
        {
            if (e.type == EventType.Repaint)
            {
                outputRects[i] = GUILayoutUtility.GetLastRect();
            }
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
        for (int i = 0; i < choices.Count; i++)
        {
            EditorGUILayout.LabelField("Option " + (i + 1));
            choices[i] = EditorGUILayout.TextArea(choices[i], GUILayout.Height(30));
        }
        EditorGUILayout.EndScrollView();

        AddChoiceRects();

        UpdateDialogueTreeElement();
        //UpdateDialogueTreeElementInfo();

        if (GUILayout.Button("Clear All", GUILayout.Height(20)))
        {
            numChoices = 1;
            inputs.Clear();
            inputRects.Clear();
            outputs.Clear();
            outputRects.Clear();
            choiceNodePair.Clear();
        }
    }

    private void AddChoiceRects()
    {
        for (int i = 0; i < numChoices; i++)
        {
            choiceRects[i] = new Rect(5, 103 + (50 * i), 290, 50);
        }
    }

    public override void SetOutput(BaseNode output, Vector2 clickPos)
    {
        clickPos.x -= windowRect.x;
        clickPos.y -= windowRect.y;

        for (int i = 0; i < choiceRects.Count; i++)
        {
            if (choiceRects[i].Contains(clickPos))
            {
                if (!choiceNodePair.ContainsKey(i))
                {
                    choiceNodePair.Add(i, output);
                    Element.ChoiceDialogueKeys.Add(i);
                    Element.ChoiceDialogueValues.Add(output.index);
                    break;
                }

                choiceNodePair[i] = output;
                Element.ChoiceDialogueValues[i] = output.index;
            }
        }

        if (outputs.Contains(output))
        {
            return;
        }

        outputs.Add(output);
        outputRects.Add(output.windowRect);
        treeElement.outputs.Add(output.ElementE);

        Element.OutputIndexes.Add(output.index);
    }

    public override void DrawCurves()
    {
        if (choiceNodePair.Count != 0)
        {
            foreach (KeyValuePair<int, BaseNode> connection in choiceNodePair)
            {
                if (connection.Value)
                {
                    Rect rect = choiceRects[connection.Key];
                    rect.x = rect.x + rect.width + windowRect.x;
                    rect.y = rect.y + (rect.height / 2) + windowRect.y;
                    rect.width = 1;
                    rect.height = 1;

                    Rect outRect = connection.Value.windowRect;
                    outRect.x = outRect.x + (outRect.width / 2);
                    outRect.y = outRect.y + (outRect.height / 2);
                    outRect.width = 1;
                    outRect.height = 1;

                    DialogueTreeEditorWindow.DrawNodeCurve(rect, outRect);
                }
            }
        }
    }

    public override void NodeDeleted(BaseNode node)
    {
        if (inputs.Contains(node))
        {
            inputs.Remove(node);
            inputRects.Remove(node.windowRect);
        }

        if (outputs.Contains(node))
        {
            outputs.Remove(node);
            outputRects.Remove(node.windowRect);
        }

        for (int i = 0; i < choiceNodePair.Count; i++)
        {
            if (node.Equals(choiceNodePair[i]))
            {
                choiceNodePair.Remove(i);

                Element.ChoiceDialogueKeys.Remove(i);
                Element.ChoiceDialogueValues.RemoveAt(i);
                break;
            }
        }
    }

    protected override void UpdateDialogueTreeElementInfo()
    {
        base.UpdateDialogueTreeElementInfo();

        ChoiceElementInfo choiceElement = (ChoiceElementInfo)dialogueTreeElement;

        choiceElement.Prompt = prompt;
        choiceElement.Choices = choices;
        choiceElement.ChoiceRects = choiceRects;
        Element = choiceElement;
    }

    protected override void UpdateDialogueTreeElementInfo(IDialogueTreeElementInfo elementInfo)
    {
        base.UpdateDialogueTreeElementInfo(elementInfo);

        ChoiceElementInfo choiceElement = (ChoiceElementInfo)dialogueTreeElement;

        choiceElement.Prompt = prompt;
        choiceElement.Choices = choices;
        choiceElement.ChoiceRects = choiceRects;
        Element = choiceElement;
    }

    protected override void UpdateDialogueTreeElement()
    {
        base.UpdateDialogueTreeElement();

        ChoiceElement choiceElement = (ChoiceElement)treeElement;

        choiceElement.choices = choices;
        treeElement = choiceElement;
    }
}
