using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    
    public class DialogueAsset : DialogueTrace
    {
        public DialogueTrace nextDialogue;
        public DialogueSettings settings;
        public DialogueData endData = new();
        
        protected override DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            return nextDialogue;
        }

        public void RunEndOperations(DialogueManager manager)
        {
            endData.RunOperations(manager);
        }
    }

}
