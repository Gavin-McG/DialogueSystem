using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Default.Runtime
{
    public class BasicOption : ChoiceOption
    {
        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return true;
        }
    }
}
