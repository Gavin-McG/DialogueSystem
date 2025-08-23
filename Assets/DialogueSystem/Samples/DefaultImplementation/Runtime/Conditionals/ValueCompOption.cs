using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Default
{
    public class ValueCompOption : Option
    {
        public ValueSO valueSO;
        public ValueComp comp;
        public float compValue;

        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return ValueUtility.CompareNumericValue(comp, valueSO, compValue, manager);
        }
    }
}