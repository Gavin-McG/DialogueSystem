using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem
{
    [OptionType("Value Comp")]
    public class ValueCompOptionType : OptionType
    {
        public DSValue dsValue;
        public DSValue.ValueComp comp;
        public float compValue;

        public override bool EvaluateCondition(AdvanceParams advanceParams, DialogueManager manager)
        {
            dsValue.TryCompareValue(manager, comp, compValue, out var result);
            return result;
        }
    }
}