using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
{
    public class BasicOption : Option
    {
        public override bool EvaluateCondition(AdvanceContext context, DialogueManager manager)
        {
            return true;
        }
    }
}
