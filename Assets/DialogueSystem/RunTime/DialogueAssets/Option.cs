namespace DialogueSystem.Runtime
{
    public abstract class Option : DialogueTrace
    {
        [HideInDialogueGraph] public DialogueTrace nextDialogue;

        protected override DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            return nextDialogue;
        }
        
        public abstract bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager);
    }
}