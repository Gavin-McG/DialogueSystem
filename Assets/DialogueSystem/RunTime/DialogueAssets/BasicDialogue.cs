using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public class BasicDialogue : DialogueTrace, IDialogueOutput
    {
        public DialogueTrace nextDialogue;
        public DialogueProfile profile;
        public string text;
        
        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context)
        {
            return nextDialogue;
        }

        public DialogueDetails GetDialogueDetails()
        {
            return new DialogueDetails()
            {
                profile = profile,
                text = text,
                isChoice = false,
                hasTimeLimit = false,
            };
        }
    }

}