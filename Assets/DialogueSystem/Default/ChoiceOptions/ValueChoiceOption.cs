using DialogueSystem.Runtime;

namespace DialogueSystem.Default.ChoiceOptions
{
    public class ValueChoiceOption : ChoiceOption
    {
        public string valueName;
        public Values.Operation operation;
        public float compValue;

        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return Values.EvaluateValue(operation, valueName, compValue, manager);
        }
    }
}