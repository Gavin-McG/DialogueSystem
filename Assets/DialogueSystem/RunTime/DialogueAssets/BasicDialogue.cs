using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public class BasicDialogue : DialogueTrace, IDialogueOutput
    {
        public DialogueTrace nextDialogue;
        public BaseParams baseParams;
        
        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            return nextDialogue;
        }

        public DialogueParams GetDialogueDetails(AdvanceDialogueContext context, DialogueManager manager)
        {
            return new DialogueParams()
            {
                dialogueType = DialogueParams.DialogueType.Basic,
                baseParams = baseParams
            };
        }
    }

}