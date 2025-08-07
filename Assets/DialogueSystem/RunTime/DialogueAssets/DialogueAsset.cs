using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    
    public class DialogueAsset : DialogueTrace
    {
        public DialogueTrace nextDialogue;
        
        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context)
        {
            return nextDialogue;
        }
    }

}
