using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of DialogueElements
/// </summary>
[System.Serializable]
public class DialogueTree : ISerializationCallbackReceiver
{
    [SerializeField]
    int index;
    DialogueTreeElement startingDialogue;

    [SerializeField]
    public List<IDialogueTreeElementInfo> serializedDialogueTree = new List<IDialogueTreeElementInfo>();

    public void OnBeforeSerialize()
    {
        if (startingDialogue == null)
        {
            startingDialogue = new DialogueElement()
            {
                sentences = new List<string>(),
                inputs = new List<DialogueTreeElement>(),
                ouputs = new List<DialogueTreeElement>(),
            };
        }
        serializedDialogueTree.Clear();
        AddNextDialogue(startingDialogue);
    }

    // TODO: Everything about this is wrong
    private void AddNextDialogue(DialogueTreeElement d)
    {
        DialogueElementInfo newD;
        ChoiceElementInfo newC;

        if (d is DialogueElement)
        {
            DialogueElement dE = d as DialogueElement;
            newD = new DialogueElementInfo()
            {
                Sentences = dE.sentences,
                InputCount = dE.inputs.Count,
                OutputCount = dE.ouputs.Count
            };
            serializedDialogueTree.Add(newD);
        }
        else if (d is ChoiceElement)
        {
            ChoiceElement cE = d as ChoiceElement;
            newC = new ChoiceElementInfo()
            {
                Choices = cE.choices,
                InputCount = cE.inputs.Count,
                OutputCount = cE.ouputs.Count
            };
            serializedDialogueTree.Add(newC);
        }

        foreach(DialogueTreeElement element in d.ouputs)
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
