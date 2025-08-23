namespace WolverineSoft.DialogueSystem.Runtime
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Base class for Conditional and Choice options. Provides the method used for each to evaluate their conditions
    /// </summary>
    public abstract class Option : DialogueTrace
    {
        [HideInDialogueGraph] public DialogueTrace nextDialogue;

        protected override DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            return nextDialogue;
        }
        
        /// <summary>
        /// Returns true if the condition is passed and the option should be considered by its respective Redirect
        /// </summary>
        public abstract bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager);
    }
}