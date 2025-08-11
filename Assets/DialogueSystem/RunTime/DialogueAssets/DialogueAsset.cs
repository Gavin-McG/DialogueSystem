using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    
    public class DialogueAsset : DialogueTrace
    {
        public DialogueTrace nextDialogue;

        public DialogueSettings settings;
        
        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context)
        {
            return nextDialogue;
        }
    }

}
