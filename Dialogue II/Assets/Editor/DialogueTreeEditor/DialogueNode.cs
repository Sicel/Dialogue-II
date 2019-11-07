using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueNode : BaseNode
{
    public List<string> sentences;
    public int numSentences;

    public DialogueElementInfo Element
    {
        get { return (DialogueElementInfo)dialogueTreeElement; }
        set { dialogueTreeElement = value; }
    }

    public DialogueElement ElementE
    {
        get => (DialogueElement)treeElement;
        set => treeElement = value;
    }

    public DialogueNode()
    {
        windowTitle = "Dialogue Node";

        numSentences = 1;

        inputs = new List<BaseNode>();
        inputRects = new List<Rect>();

        outputs = new List<BaseNode>();
        outputRects = new List<Rect>();

        sentences = new List<string>();

        treeElement = new DialogueElement()
        {
            inputs = new List<DialogueTreeElement>(),
            ouputs = new List<DialogueTreeElement>(),
            sentences = new List<string>()
        };

        dialogueTreeElement = new DialogueElementInfo()
        {
            WindowRect = windowRect,
            Index = index,
            InputIndexes = new List<int>(),
            InputRects = inputRects,
            OutputIndexes = new List<int>(),
            OutputRects = outputRects,
            Sentences = sentences
        };
    }

    public override void DrawWindow()
    {
        base.DrawWindow();

        Event e = Event.current;

        numSentences = EditorGUILayout.IntField("Number of Snetences", numSentences);
        if (numSentences < 0)
        {
            numSentences = 0;
        }

        EditorGUILayout.LabelField("Dialogue:");
        EditorGUILayout.Space();

        int difference = Mathf.Abs(numSentences - sentences.Count);
        if (sentences.Count < numSentences)
        {
            for (int i = 0; i < difference; i++)
            {
                sentences.Add("Enter Dialogue Here");
            }
        }
        else if (sentences.Count > numSentences)
        {
            if (numSentences != 0)
            {
                sentences.RemoveRange(numSentences - 1, difference);
            }
            else
            {
                sentences.Clear();
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
        for (int i = 0; i < sentences.Count; i++)
        {
            sentences[i] = EditorGUILayout.TextArea(sentences[i], GUILayout.Height(30));
        }
        EditorGUILayout.EndScrollView();

        UpdateDialogueTreeElement();

        if (GUILayout.Button("Clear All", GUILayout.Height(20)))
        {
            numSentences = 0;
            sentences.Clear();
            //inputs.Clear();
            //inputRects.Clear();
            //outputs.Clear();
            //outputRects.Clear();
        }
    }

    public override void SetOutput(BaseNode output, Vector2 clickPos)
    {
        if (outputs.Contains(output))
        {
            return;
        }

        outputs.Add(output);
        outputRects.Add(output.windowRect);
        Element.OutputIndexes.Add(output.index);
        Element.OutputRects.Add(output.windowRect);
    }

    public override void DrawCurves()
    {
        for (int i = 0; i < outputs.Count; i++)
        {
            if (outputs[i])
            {
                Rect rect = windowRect;
                rect.x = rect.x + (rect.width / 2);
                rect.y = rect.y + (rect.height / 2);
                rect.width = 1;
                rect.height = 1;

                Rect outRect = outputs[i].windowRect;
                outRect.x = outRect.x + (outRect.width / 2);
                outRect.y = outRect.y + (outRect.height / 2);
                outRect.width = 1;
                outRect.height = 1;

                DialogueTreeEditorWindow.DrawNodeCurve(rect, outRect);
            }
        }
    }

    public override void NodeDeleted(BaseNode node)
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            if (node.Equals(inputs[i]))
            {
                inputs.Remove(inputs[i]);
                inputRects.Remove(inputRects[i]);

                Element.InputIndexes.Remove(Element.InputIndexes[i]);
                Element.InputRects.Remove(Element.InputRects[i]);
                break;
            }
        }

        for (int i = 0; i < outputs.Count; i++)
        {
            if (node.Equals(outputs[i]))
            {
                outputs.Remove(outputs[i]);
                inputRects.Remove(inputRects[i]);

                Element.OutputIndexes.Remove(Element.OutputIndexes[i]);
                Element.OutputRects.Remove(Element.OutputRects[i]);
                break;
            }
        }
    }

    protected override void UpdateDialogueTreeElement()
    {
        base.UpdateDialogueTreeElement();

        DialogueElementInfo dialogueElement = (DialogueElementInfo)dialogueTreeElement;
        dialogueElement.Sentences = sentences;

        Element = dialogueElement;
    }
}
