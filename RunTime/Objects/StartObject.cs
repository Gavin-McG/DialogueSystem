using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    [Serializable]
    public class StartObject : DialogueObject
    {
        [SerializeField] public DialogueObject nextDialogue;
        [SerializeField] public string startName;
        [SerializeReference] public DialogueParameters dialogueParameters;

        public override DialogueObject GetNextDialogue(AdvanceContext advanceContext, DialogueManager manager)
        {
            return nextDialogue;
        }
    }
}
