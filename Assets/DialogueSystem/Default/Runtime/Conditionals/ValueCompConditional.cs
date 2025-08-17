using DialogueSystem.Runtime;

namespace DialogueSystem.Default.Runtime
{
    public class ValueCompConditional : ConditionalOption
    {
        public string valueName;
        public Values.CompOperation operation;
        public float compValue;

        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return Values.CompareNumericValue(operation, valueName, compValue, manager);
        }
    }
}