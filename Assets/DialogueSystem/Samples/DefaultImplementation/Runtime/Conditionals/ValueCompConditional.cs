using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Default
{
    public class ValueCompConditional : Option
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