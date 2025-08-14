using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public class ChoiceDialogue : DialogueTrace, IDialogueOutput
    {
        public DialogueTrace defaultDialogue;
        public BaseParams baseParams;
        public ChoiceParams choiceParams;
        public List<ChoiceOption> options;
        

        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            if (context.timedOut) 
                return defaultDialogue;
            
            if (context.choice >= options.Count)
            {
                throw new System.Exception("Choice out of range");
            }
            return options[context.choice];
        }

        public DialogueParams GetDialogueDetails()
        {
            return new DialogueParams()
            {
                dialogueType = DialogueParams.DialogueType.Choice,
                baseParams = baseParams,
                choiceParams = choiceParams,
                choicePrompts = options.Select(option => option.optionParams).ToList()
            };
        }
    }

}