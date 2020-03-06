using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of DialogueElements
/// </summary>
[Serializable]
public class DialogueTree : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField]
    int index;
    public DialogueTreeElement startingDialogue;
    public List<DialogueTreeElement> dialogues = new List<DialogueTreeElement>();
    public List<IDialogueTreeElementInfo> serializedDialogueTree = new List<IDialogueTreeElementInfo>();

    public List<DialogueTreeElement> Dialogues
    {
        get
        {
            if (dialogues.Count == 0)
            {
                if (startingDialogue == null)
                {
                    startingDialogue = new DialogueElement();
                }

                dialogues.Add(startingDialogue);
            }

            return dialogues;
        }

        set => dialogues = value;
    }


    #region Serialization
    public void OnBeforeSerialize()
    {
        if (startingDialogue == null)
        {
            startingDialogue = new DialogueElement();
        }
        serializedDialogueTree.Clear();
        AddNextDialogue(startingDialogue);
    }

    private void AddNextDialogue(DialogueTreeElement d)
    {
        DialogueElementInfo dialogueElementInfo;
        ChoiceElementInfo choiceElementInfo;

        switch (d.DialogueType)
        {
            case DialogueType.Dialogue:
                dialogueElementInfo = new DialogueElementInfo()
                {
                    WindowRect = new Rect(0, 0, 300, 300),
                    InputIndexes = new List<int>(),
                    InputRects = new List<Rect>(),
                    OutputIndexes = new List<int>(),
                    OutputRects = new List<Rect>(),

                    Sentences = new List<string>()
                };
                break;
            case DialogueType.Choice:
                choiceElementInfo = new ChoiceElementInfo()
                {
                    WindowRect = new Rect(0, 0, 300, 300),
                    InputIndexes = new List<int>(),
                    InputRects = new List<Rect>(),
                    OutputIndexes = new List<int>(),
                    OutputRects = new List<Rect>(),

                    Choices = new List<string>(),
                    ChoiceRects = new List<Rect>(),
                    ChoiceDialogueKeys = new List<int>(),
                    ChoiceDialogueValues = new List<int>()
                };
                break;
        }

        serializedDialogueTree.Add(d.ElementInfo);

        foreach (DialogueTreeElement element in d.outputs)
        {
            AddNextDialogue(element);
        }
    }

    public void OnAfterDeserialize()
    {
        if (serializedDialogueTree.Count > 0)
        {
            ReadDialogueFromSerialized(0, out startingDialogue);
        }
        else
        {
            startingDialogue = new DialogueElement();
        }
    }

    private int ReadDialogueFromSerialized(int index, out DialogueTreeElement d)
    {
        IDialogueTreeElementInfo elementInfo = serializedDialogueTree[index];

        DialogueElement dialogue = null;
        ChoiceElement choice = null;

        if (elementInfo is DialogueElementInfo)
        {
            DialogueElementInfo info = (DialogueElementInfo)elementInfo;
            dialogue = new DialogueElement()
            {
                sentences = info.Sentences,
                inputs = new List<DialogueTreeElement>()
            };
        }
        else if (elementInfo is ChoiceElementInfo)
        {
            ChoiceElementInfo info = (ChoiceElementInfo)elementInfo;
            choice = new ChoiceElement()
            {
                choices = info.Choices,
                inputs = new List<DialogueTreeElement>()
            };
        }

        DialogueTreeElement child;
        index = ReadDialogueFromSerialized(++index, out child);

        if (dialogue != null)
        {
            dialogue.inputs.Add(child);
            d = dialogue;
        }
        else
        {
            choice.inputs.Add(child);
            d = choice;
        }

        return index;
    }
    #endregion
}
