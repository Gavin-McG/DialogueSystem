using DialogueSystem.Runtime;

namespace DialogueSystem.Default.ChoiceOptions.Editor
{
    public class BasicChoiceOption : ChoiceOption
    {
        public override bool DisplayChoice(AdvanceDialogueContext context, DialogueManager manager)
        {
            return true;
        }
    }
}
