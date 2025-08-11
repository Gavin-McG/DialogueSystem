using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public abstract class DialogueTrace : ScriptableObject
    {
        public List<DSEventCaller> events;
        
        public abstract DialogueTrace GetNextDialogue(AdvanceDialogueContext context);

        public void InvokeEvents()
        {
            foreach (var dialogueEvent in events)
            {
                dialogueEvent.Invoke();
            }
        }
    }

}
