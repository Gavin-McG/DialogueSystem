using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    public class RandomRedirect : Redirect
    {
        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            var validOptions = options
                .Where(option => option.EvaluateCondition(context, manager))
                .ToList();

            if (validOptions.Count == 0)
                return defaultDialogue;

            var totalWeight = validOptions.Sum(o => Mathf.Max(o.weight, 0f));
            var randomValue = UnityEngine.Random.value * totalWeight;

            var cumulative = 0f;
            foreach (var option in validOptions)
            {
                cumulative += option.weight;
                if (randomValue <= cumulative)
                    return option;
            }

            return validOptions[^1];
        }
    }
}