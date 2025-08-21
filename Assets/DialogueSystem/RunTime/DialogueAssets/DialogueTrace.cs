using System;
using System.Collections.Generic;
using DialogueSystem.Runtime.Keywords;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public abstract class DialogueTrace : ScriptableObject
    {
        [SerializeField] public DialogueData data = new();

        public DialogueTrace AdvanceDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            data.RunOperations(manager);
            return GetNextDialogue(context, manager);
        }
        
        protected abstract DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager);
    }

}
