﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serializable dialogue node
/// </summary>
[System.Serializable]
public struct DialogueElementInfo : IDialogueTreeElementInfo
{
    // Interface properties
    #region Interface Properties
    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.Index"/>
    /// </summary>
    public int Index { get; set; }

    // TODO: Replace with window position
    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.WindowRect"/>
    /// </summary>
    public Rect WindowRect { get; set; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.InputCount"/>
    /// </summary>
    public int InputCount { get => InputIndexes.Count; }

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
    public int OutputCount { get => OutputIndexes.Count; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.OutputRects"/>
    /// </summary>
    public List<Rect> OutputRects { get; set; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.OutputIndexes"/>
    /// </summary>
    public List<int> OutputIndexes { get; set; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.HasInputs"/>
    /// </summary>
    public bool HasInputs { get => InputCount > 0 ? true : false; }

    /// <summary>
    /// See <see cref="IDialogueTreeElementInfo.HasOutputs"/>
    /// </summary>
    public bool HasOutputs { get => OutputCount > 0 ? true : false; }
    #endregion

    #region Dialogue Properties
    /// <summary>
    /// Dialogue that is displayed
    /// </summary>
    public List<string> Sentences { get; set; }
    #endregion
}
