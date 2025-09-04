
namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Redirect Object that evaluates conditions until the first pass
    /// </summary>
    public sealed class SequentialRedirect : Redirect
    {
        protected override DialogueTrace GetNextDialogue(AdvanceParams advanceParams, DialogueManager manager)
        {
            foreach (var option in options)
            {
                if (option.EvaluateCondition(advanceParams, manager)) return option;
            }
            return defaultDialogue;
        }
    }

}