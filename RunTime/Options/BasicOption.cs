using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem
{
    [TypeOption("Basic")]
    public class BasicOption : OptionType
    {
        public override bool EvaluateCondition(AdvanceContext advanceContext, IVariableContext variables)
        {
            return true;
        }
    }
}
