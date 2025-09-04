using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
{
    public class BasicOption : Option
    {
        public override bool EvaluateCondition(AdvanceParams advanceParams, DialogueManager manager)
        {
            return true;
        }
    }
}
