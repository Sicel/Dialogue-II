using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class BaseNode : ScriptableObject
{
    public Rect windowRect; // Node window
    public int index; // Location in list

    public IDialogueTreeElementInfo dialogueTreeElement;
    public DialogueTreeElement treeElement;

    // Inputs
    public List<BaseNode> inputs;
    public List<Rect> inputRects;

    // Outputs
    public List<BaseNode> outputs;
    public List<Rect> outputRects;

    public string windowTitle; // Title of window
    protected Vector2 scrollPos;

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
        dialogueTreeElement.InputIndexes.Add(input.index);
        dialogueTreeElement.InputRects.Add(input.windowRect);
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

    protected virtual void UpdateDialogueTreeElement()
    {
        dialogueTreeElement.WindowRect = windowRect;
        dialogueTreeElement.Index = index;
        dialogueTreeElement.InputRects = inputRects;
        dialogueTreeElement.OutputRects = outputRects;
    }
}
