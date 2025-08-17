using DialogueSystem.Runtime;

namespace DialogueSystem.Default.Runtime
{
    public class ValueCompOption : ChoiceOption
    {
        public string valueName;
        public Values.Operation operation;
        public float compValue;

        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return Values.CompareNumericValue(operation, valueName, compValue, manager);
        }
    }
}