using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Default
{
    public class ValueCompOption : Option
    {
        public DSValue valueSO;
        public DSValue.ValueComp comp;
        public float compValue;

        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            valueSO.TryCompareValue(manager, comp, compValue, out var result);
            return result;
        }
    }
}