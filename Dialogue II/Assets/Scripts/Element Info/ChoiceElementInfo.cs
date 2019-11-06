using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serializable Choice Node
/// </summary>
[System.Serializable]
public struct ChoiceElementInfo : IDialogueTreeElementInfo
{
    #region Interface Properties
    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.Index"/>
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.WindowRect"/>
    /// </summary>
    public Rect WindowRect { get; set; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.InputCount"/>
    /// </summary>
    public int InputCount { get; set; }
    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.InputRects"/>
    /// </summary>
    public List<Rect> InputRects { get; set; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.InputIndexes"/>
    /// </summary>
    public List<int> InputIndexes { get; set; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.OutputCount"/>
    /// </summary>
    public int OutputCount { get; set; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.OutputRects"/>
    /// </summary>
    public List<Rect> OutputRects { get; set; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.OutputIndexes"/>
    /// </summary>
    public List<int> OutputIndexes { get; set; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.IndexofFirstInput"/>
    /// </summary>
    public int IndexofFirstInput { get; set; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.IndexOfFirstOutput"/>
    /// </summary>
    public int IndexOfFirstOutput { get; set; }
    #endregion

    #region Choice Properties
    /// <summary>
    /// Dialogue that accompanies the choices
    /// </summary>
    public string Prompt { get; set; }

    /// <summary>
    /// Choices the player is able to choose from
    /// </summary>
    public List<string> Choices { get; set; }

    /// <summary>
    /// Number of choices
    /// </summary>
    public int NumChoices { get; set; }
    
    /// <summary>
    /// Rect for choice nodes in Dialogue Tree Editor Window
    /// </summary>
    public List<Rect> ChoiceRects { get; set; }

    /// <summary>
    /// Choice numbers
    /// </summary>
    public List<int> ChoiceDialogueKeys { get; set; }

    /// <summary>
    /// Index of node corresponding with choice
    /// </summary>
    public List<int> ChoiceDialogueValues { get; set; }
    #endregion
}
