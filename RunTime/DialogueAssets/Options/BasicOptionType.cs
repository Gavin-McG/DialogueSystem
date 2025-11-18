using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem
{
    [OptionType("Basic")]
    public class BasicOptionType : OptionType
    {
        public override bool EvaluateCondition(AdvanceParams advanceParams, DialogueManager manager)
        {
            return true;
        }
    }
}
