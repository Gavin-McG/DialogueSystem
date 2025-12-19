using System.Collections.Generic;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    public class SequentialRedirectObject : RedirectObject
    {
        public override DialogueObject GetNextDialogue(AdvanceContext advanceContext, DialogueManager manager)
        {
            foreach (OptionObject option in options)
            {
                if (option.EvaluateCondition(advanceContext, manager)) return option;
            }
            
            return defaultDialogue;
        }
    }
}