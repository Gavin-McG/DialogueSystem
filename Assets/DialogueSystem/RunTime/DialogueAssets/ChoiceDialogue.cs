using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public class ChoiceDialogue : DialogueTrace, IDialogueOutput
    {
        public DialogueTrace defaultDialogue;
        public DialogueProfile profile;
        public string text;
        public bool hasTimeLimit = false;
        public float timeLimitDuration = 0;
        public List<ChoiceOption> options;
        

        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context)
        {
            if (context.timedOut) 
                return defaultDialogue;
            
            if (context.choice >= options.Count)
            {
                throw new System.Exception("Choice out of range");
            }
            return options[context.choice];
        }

        public DialogueDetails GetDialogueDetails()
        {
            return new DialogueDetails()
            {
                profile = profile,
                text = text,
                isChoice = true,
                choicePrompts = options.Select((option => option.prompt)).ToList(),
                hasTimeLimit = hasTimeLimit,
                timeLimitDuration = timeLimitDuration
            };
        }
    }

}