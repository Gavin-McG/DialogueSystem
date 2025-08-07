using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    public class ChoiceOption : DialogueTrace
    {
        public DialogueTrace nextDialogue;
        public string prompt;
        
        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context)
        {
            return nextDialogue;
        }
    }

}
