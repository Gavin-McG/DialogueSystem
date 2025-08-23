using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Default.Runtime
{
    public class BasicOption : Option
    {
        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return true;
        }
    }
}
