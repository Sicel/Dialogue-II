using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Choice node
/// </summary>
public class ChoiceElement : DialogueTreeElement
{
    new public ChoiceElementInfo ElementInfo
    {
        get => (ChoiceElementInfo)base.ElementInfo;
        set => base.ElementInfo = value;
    }
    public List<string> choices = new List<string>();
}
