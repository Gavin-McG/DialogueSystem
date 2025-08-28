using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
{
    public class BasicOption : Option
    {
        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return true;
        }
    }
}
