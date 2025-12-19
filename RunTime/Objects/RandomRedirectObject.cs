using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    public class RandomRedirectObject : RedirectObject
    {
        public override DialogueObject GetNextDialogue(AdvanceContext advanceContext, DialogueManager manager)
        {
            //Get all options who pass their condition
            List<OptionObject> passedOptions = new List<OptionObject>();
            foreach (OptionObject option in options)
            {
                if (option.EvaluateCondition(advanceContext, manager))
                    passedOptions.Add(option);
            }
            
            //Select a Weighted random selection from all passing Options
            var weightSum = passedOptions.Aggregate(0f, (sum, option) => sum + option.weight);
            var randomVal = Random.Range(0, weightSum);
            foreach (var option in passedOptions)
            {
                randomVal -= option.weight;
                if (randomVal <= 0) return option;
            }
            
            return defaultDialogue;
        }
    }
}