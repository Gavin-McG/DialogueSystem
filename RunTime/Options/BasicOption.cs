using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem
{
    public class BasicOption : OptionType
    {
        public override bool EvaluateCondition(AdvanceContext advanceContext, DialogueManager manager)
        {
            return true;
        }
    }
}
