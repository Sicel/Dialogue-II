using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dialogue node
/// </summary>
public class DialogueElement : DialogueTreeElement
{
    new public DialogueElementInfo ElementInfo
    {
        get => (DialogueElementInfo)base.ElementInfo;
        set => base.ElementInfo = value;
    }

    public List<string> sentences = new List<string>();
}
