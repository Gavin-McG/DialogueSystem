using System.Collections.Generic;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Redirect Object that evaluates conditions until the first pass
    /// </summary>
    public sealed class SequentialRedirect : Redirect
    {
        protected override DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            foreach (var option in options)
            {
                if (option.EvaluateCondition(context, manager)) return option;
            }
            return defaultDialogue;
        }
    }

}