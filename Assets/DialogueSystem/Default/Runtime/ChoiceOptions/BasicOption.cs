using DialogueSystem.Runtime;

namespace DialogueSystem.Default.Runtime
{
    public class BasicOption : ChoiceOption
    {
        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return true;
        }
    }
}
