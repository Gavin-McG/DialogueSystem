using System.Linq;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Redirect Object that evaluates all conditions and selects randomly based on weight values.
    /// </summary>
    public sealed class RandomRedirect : Redirect
    {
        protected override DialogueTrace GetNextDialogue(AdvanceContext context, DialogueManager manager)
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