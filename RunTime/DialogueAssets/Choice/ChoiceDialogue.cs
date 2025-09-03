using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Scriptable Object representing a Choice Dialogue
    /// </summary>
    public sealed class ChoiceDialogue : DialogueTrace, IDialogueOutput
    {
        public DialogueTrace defaultDialogue;
        [SerializeReference] public BaseParams baseParams;
        [SerializeReference] public ChoiceParams choiceParams;
        public List<Option> options;
        

        protected override DialogueTrace GetNextDialogue(AdvanceContext context, DialogueManager manager)
        {
            if (context.timedOut) 
                return defaultDialogue;
            
            if (context.choice >= manager.optionIndexes.Count)
                throw new System.Exception("Choice out of range");
            
            int optionIndex = manager.optionIndexes[context.choice];
            return options[optionIndex];
        }

        public DialogueParams GetDialogueDetails(AdvanceContext context, DialogueManager manager)
        {
            // Get both the option and its original index
            var filteredOptions = options
                .Select((option, index) => (option, index))
                .Where(x => x.option.EvaluateCondition(context, manager))
                .ToList();

            // Store the indexes in the manager
            manager.optionIndexes = filteredOptions.Select(x => x.index).ToList();

            // Build the choice prompts from the filtered options
            return new DialogueParams(baseParams, choiceParams, 
                filteredOptions.Select(x => x.option.optionParams).ToList());
        }
    }

}