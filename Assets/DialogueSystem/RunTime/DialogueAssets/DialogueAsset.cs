using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    
    public class DialogueAsset : DialogueTrace
    {
        public DialogueTrace nextDialogue;
        public DialogueSettings settings;
        public List<DSEventCaller> endEvents = new List<DSEventCaller>();
        
        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            return nextDialogue;
        }
        
        public void InvokeEndEvents()
        {
            foreach (var dialogueEvent in endEvents)
            {
                dialogueEvent.Invoke();
            }
        }
    }

}
