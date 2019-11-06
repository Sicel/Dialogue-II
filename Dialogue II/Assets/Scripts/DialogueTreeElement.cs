using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A node in the dialogue tree
/// </summary>
public class DialogueTreeElement
{
    //public int index;
    public List<DialogueTreeElement> inputs = new List<DialogueTreeElement>();
    public List<DialogueTreeElement> ouputs = new List<DialogueTreeElement>();
}
