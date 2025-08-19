using DialogueSystem.Runtime;
using DialogueSystem.Runtime.Values;

namespace DialogueSystem.Default.Runtime
{
    public class ValueCompConditional : ConditionalOption
    {
        public string valueName;
        public ValueComp comp;
        public float compValue;

        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return ValueUtility.CompareNumericValue(comp, valueName, compValue, manager);
        }
    }
}