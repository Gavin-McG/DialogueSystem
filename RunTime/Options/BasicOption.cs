using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem
{
    [TypeOption("Basic", order = -1)]
    public class BasicOption : OptionType
    {
        public override bool EvaluateCondition(AdvanceContext advanceContext, IVariableContext variables)
        {
            return true;
        }
    }
}
