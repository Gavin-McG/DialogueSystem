using UnityEngine;

namespace DialogueSystem.Runtime
{

    public abstract class ConditionalOption : DialogueTrace
    {
        [HideInDialogueGraph] public DialogueTrace nextDialogue;
        [HideInDialogueGraph] public float weight = 1;

        public override DialogueTrace GetNextDialogue(AdvanceDialogueContext context)
        {
            return nextDialogue;
        }

        public abstract bool EvaluateCondition(AdvanceDialogueContext context);
    }

}
