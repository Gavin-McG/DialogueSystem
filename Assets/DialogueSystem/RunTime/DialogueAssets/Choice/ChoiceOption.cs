using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    public class ChoiceOption : DialogueTrace
    {
        public DialogueTrace nextDialogue;
        public OptionParams optionParams;
        
        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context)
        {
            return nextDialogue;
        }
    }

}
