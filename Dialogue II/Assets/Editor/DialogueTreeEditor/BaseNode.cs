using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class BaseNode : ScriptableObject
{
    public Rect windowRect; // Node window
    public int index; // Location in list

    public IDialogueTreeElementInfo dialogueTreeElement;
    protected DialogueTreeElement treeElement;

    public DialogueTreeElement ElementE
    {
        get
        {
            if (this is DialogueNode)
            {
                if (treeElement == null)
                    treeElement = new DialogueElement();

                return (DialogueElement)treeElement;
            }
            else if (this is ChoiceNode)
            {
                if (treeElement == null)
                    treeElement = new ChoiceElement();

                return (ChoiceElement)treeElement;
            }
            return treeElement;
        }
        set => treeElement = value;
    }

    // Inputs
    public List<BaseNode> inputs;
    public List<Rect> inputRects;

    // Outputs
    public List<BaseNode> outputs;
    public List<Rect> outputRects;

    public string windowTitle; // Title of window
    protected Vector2 scrollPos;

    /// <summary>
    /// Creates a new node using a DialogueTreeElement. Used in absense of parameterized constructor
    /// </summary>
    /// <param name="element"></param>
    public virtual void Init(DialogueTreeElement element)
    {
        windowRect = new Rect(element.ElementInfo.WindowRect.x, element.ElementInfo.WindowRect.y, 300, 300);

        index = element.ElementInfo.Index;

        if (element.ElementInfo.HasInputs)
            inputRects = element.ElementInfo.InputRects;
        if (element.ElementInfo.HasOutputs)
            outputRects = element.ElementInfo.OutputRects;

        treeElement = element;
    }

    /// <summary>
    /// Draws node window
    /// </summary>
    public virtual void DrawWindow()
    {
        windowTitle = EditorGUILayout.TextField("New Node", windowTitle);
    }

    /// <summary>
    /// Draws curves from nodes
    /// </summary>
    public abstract void DrawCurves();

    /// <summary>
    /// Adds node to input list
    /// </summary>
    /// <param name="input">Node being added to input</param>
    /// <param name="clickPos">Where mouse clicked</param>
    public virtual void SetInput(BaseNode input, Vector2 clickPos)
    {
        clickPos.x -= windowRect.x;
        clickPos.y -= windowRect.y;

        if (inputs.Contains(input))
        {
            return;
        }

        SetInput(input);
    }

    /// <summary>
    /// Adds node as to input
    /// </summary>
    /// <param name="input">Node being added as input</param>
    public virtual void SetInput(BaseNode input)
    {
        inputs.Add(input);
        inputRects.Add(input.windowRect);
        treeElement.inputs.Add(input.ElementE);
        treeElement.ElementInfo.InputIndexes.Add(input.index);
        treeElement.ElementInfo.InputRects.Add(input.windowRect);
        //dialogueTreeElement.InputIndexes.Add(input.index);
        //dialogueTreeElement.InputRects.Add(input.windowRect);
    }

    /// <summary>
    /// Adds node to output list
    /// </summary>
    /// <param name="output">Node being added to outputs</param>
    /// <param name="clickPos">Where mouse clicked</param>
    public virtual void SetOutput(BaseNode output, Vector2 clickPos) { }

    /// <summary>
    /// Removes all references to node being deleted
    /// </summary>
    /// <param name="node">Node being deleted</param>
    public virtual void NodeDeleted(BaseNode node) { }

    protected virtual void UpdateDialogueTreeElementInfo()
    {
        dialogueTreeElement.WindowRect = windowRect;
        dialogueTreeElement.Index = index;
        dialogueTreeElement.InputRects = inputRects;
        dialogueTreeElement.OutputRects = outputRects;
    }

    protected virtual void UpdateDialogueTreeElementInfo(IDialogueTreeElementInfo elementInfo)
    {
        elementInfo.WindowRect = windowRect;
        elementInfo.Index = index;

        if (inputs != null)
        {
            if (inputs.Count > 0)
                elementInfo.IndexofFirstInput = inputs[0].index;

            elementInfo.InputIndexes.Clear();
            elementInfo.InputRects.Clear();

            for (int i = 0; i < inputs.Count; i++)
                elementInfo.InputIndexes.Add(inputs[i].index);

            elementInfo.InputRects = inputRects;
        }

        if (outputs != null)
        {
            if (outputs.Count > 0)
                elementInfo.IndexofFirstInput = outputs[0].index;

            elementInfo.OutputIndexes.Clear();
            elementInfo.OutputRects.Clear();

            for (int i = 0; i < outputs.Count; i++)
                elementInfo.InputIndexes.Add(outputs[i].index);

            elementInfo.OutputRects = outputRects;
        }
    }

    protected virtual void UpdateDialogueTreeElement()
    {
        treeElement.inputs.Clear();
        for (int i = 0; i < inputs.Count; i++)
        {
            treeElement.inputs.Add(inputs[i].ElementE);
        }

        treeElement.outputs.Clear();
        for (int i = 0; i < outputs.Count; i++)
        {
            treeElement.outputs.Add(outputs[i].ElementE);
        }

        UpdateDialogueTreeElementInfo(treeElement.ElementInfo);
    }
}
