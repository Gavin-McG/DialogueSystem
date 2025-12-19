using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Scriptable Object representing a Choice Dialogue
    /// </summary>
    public class ChoiceObject : DialogueObject, IDialogueOutput
    {
        [SerializeField] public DialogueObject defaultDialogue;
        [SerializeField] public string text;
        [SerializeReference] public TextParameters textParams;
        [SerializeReference] public ChoiceParameters choiceParams;
        [SerializeField] public List<OptionObject> options;

        public override DialogueObject GetNextDialogue(AdvanceContext advanceContext, DialogueManager manager)
        {
            if (advanceContext.choice == -1) 
                return defaultDialogue;
            
            if (advanceContext.choice >= manager.optionIndexes.Count || advanceContext.choice < 0)
                throw new System.Exception("Choice out of range");
            
            int optionIndex = manager.optionIndexes[advanceContext.choice];
            return options[optionIndex];
        }

        public DialogueInfo GetDialogueDetails(AdvanceContext advanceContext, DialogueManager manager)
        {
            // Get both the option and its original index
            var filteredOptions = options
                .Select((option, index) => (option, index))
                .Where(x => x.option.EvaluateCondition(advanceContext, manager))
                .ToList();

            // Store the indexes in the manager
            manager.optionIndexes = filteredOptions.Select(x => x.index).ToList();

            // Build the choice prompts from the filtered options
            return new DialogueInfo(text, textParams, choiceParams, 
                filteredOptions.Select(x => 
                    new ResponseInfo(x.option.text, x.option.responseParams)
                ).ToList());
        }
    }

}