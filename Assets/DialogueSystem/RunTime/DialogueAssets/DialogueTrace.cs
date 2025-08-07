using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public abstract class DialogueTrace : DialogueObject
    {
        public List<DialogueEvent> events;
        
        public abstract DialogueTrace GetNextDialogue(AdvanceDialogueContext context);

        public void InvokeEvents()
        {
            foreach (var dialogueEvent in events)
            {
                dialogueEvent.dialogueEvent.Invoke();
            }
        }
    }

}
