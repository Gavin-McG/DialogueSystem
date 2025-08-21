using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public class Dialogue : DialogueTrace, IDialogueOutput
    {
        public DialogueTrace nextDialogue;
        [SerializeReference] public BaseParams baseParams;
        
        protected override DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            return nextDialogue;
        }

        public DialogueParams GetDialogueDetails(AdvanceDialogueContext context, DialogueManager manager)
        {
            return new DialogueParams(baseParams);
        }
    }

}