using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public class SequentialRedirect : Redirect
    {
        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context)
        {
            foreach (var option in options)
            {
                if (option.EvaluateCondition(context)) return option;
            }
            return defaultDialogue;
        }
    }

}