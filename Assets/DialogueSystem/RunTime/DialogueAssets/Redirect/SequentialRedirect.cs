using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public class SequentialRedirect : Redirect
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