using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of DialogueElements
/// </summary>
[Serializable]
public class DialogueTree : ISerializationCallbackReceiver
{
    [SerializeField]
    int index;
    public DialogueTreeElement startingDialogue;
    public List<DialogueTreeElement> dialogues = new List<DialogueTreeElement>();

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

    [SerializeField]
    public List<IDialogueTreeElementInfo> serializedDialogueTree = new List<IDialogueTreeElementInfo>();

    public void OnBeforeSerialize()
    {
        serializedDialogueTree.Clear();
        AddNextDialogue(Dialogues[0]);
    }

    private void AddNextDialogue(DialogueTreeElement d)
    {
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
}
