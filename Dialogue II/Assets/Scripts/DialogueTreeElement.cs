using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueType
{
    Dialogue,
    Choice
}

/// <summary>
/// A node in the dialogue tree
/// </summary>
public abstract class DialogueTreeElement
{
    //public int index;
    protected IDialogueTreeElementInfo elementInfo;
    public IDialogueTreeElementInfo ElementInfo
    {
        get
        {
            if (this is DialogueElement)
            {
                if (elementInfo == null)
                {
                    elementInfo = new DialogueElementInfo()
                    {
                        InputIndexes = new List<int>(),
                        InputRects = new List<Rect>(),
                        OutputIndexes = new List<int>(),
                        OutputRects = new List<Rect>(),
                        Sentences = new List<string>()
                    };
                }

                return (DialogueElementInfo)elementInfo;
            }
            else if (this is ChoiceElement)
            {
                if (elementInfo == null)
                {
                    elementInfo = new ChoiceElementInfo()
                    {
                        InputIndexes = new List<int>(),
                        InputRects = new List<Rect>(),
                        OutputIndexes = new List<int>(),
                        OutputRects = new List<Rect>(),
                        Choices = new List<string>(),
                        ChoiceRects = new List<Rect>(),
                        ChoiceDialogueKeys = new List<int>(),
                        ChoiceDialogueValues = new List<int>()
                    };
                }

                return (ChoiceElementInfo)elementInfo;
            }

            return elementInfo;
        }
        set => elementInfo = value;
    }

    public DialogueType DialogueType
    {
        get
        {
            if (this is DialogueElement)
                dialogueType = DialogueType.Dialogue;
            else if (this is ChoiceElement)
                dialogueType = DialogueType.Choice;

            return dialogueType;
        }
    }

    public List<DialogueTreeElement> inputs = new List<DialogueTreeElement>();
    public List<DialogueTreeElement> outputs = new List<DialogueTreeElement>();
    private DialogueType dialogueType;
}
