using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    public abstract class ChoiceOption : DialogueTrace
    {
        [HideInDialogueGraph] public DialogueTrace nextDialogue;
        [HideInDialogueGraph] public OptionParams optionParams;
        
        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            return nextDialogue;
        }

        public abstract bool DisplayChoice(AdvanceDialogueContext context, DialogueManager manager);
    }

}
