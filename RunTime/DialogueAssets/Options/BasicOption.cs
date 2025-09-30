using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem
{
    public class BasicOption : Option
    {
        public override bool EvaluateCondition(AdvanceParams advanceParams, DialogueManager manager)
        {
            return true;
        }
    }
}
