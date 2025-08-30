
namespace WolverineSoft.DialogueSystem
{
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