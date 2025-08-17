using DialogueSystem.Runtime;

namespace DialogueSystem.Default.Runtime
{
    public class BasicChoiceOption : ChoiceOption
    {
        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return true;
        }
    }
}
