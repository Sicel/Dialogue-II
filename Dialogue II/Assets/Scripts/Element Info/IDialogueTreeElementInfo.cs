using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base for serializable dialogue tree elements
/// </summary>
public interface IDialogueTreeElementInfo
{
    #region Properties
    /// <summary>
    /// Is this struct empty
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// Location in dialogue tree
    /// </summary>
    int Index { get; set; }
    /// <summary>
    /// Rect of node in Dialogue Tree Editor Window
    /// </summary>
    Rect WindowRect { get; set; }

    /// <summary>
    /// Number of inputs connected to node
    /// </summary>
    int InputCount { get; }

    /// <summary>
    /// Rect of inputs in Dialogue Tree Editor Window
    /// </summary>
    List<Rect> InputRects { get; set; }

    /// <summary>
    /// Location of inputs in dialogue tree
    /// </summary>
    List<int> InputIndexes { get; set; }

    /// <summary>
    /// Number of outputs node has
    /// </summary>
    int OutputCount { get; }

    /// <summary>
    /// Rect of outputs in Dialogue Tree Editor Window
    /// </summary>
    List<Rect> OutputRects { get; set; }

    /// <summary>
    /// Location of outputs in dialogue tree
    /// </summary>
    List<int> OutputIndexes { get; set; }

    /// <summary>
    /// Does this node have any inputs?
    /// </summary>
    bool HasInputs { get; }

    /// <summary>
    /// Does node have any outputs?
    /// </summary>
    bool HasOutputs { get; }
    #endregion
}