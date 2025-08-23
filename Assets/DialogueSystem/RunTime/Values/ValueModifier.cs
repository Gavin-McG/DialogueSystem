using System;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// ValueEditor which performs a mathematical operation on a numeric value
    /// </summary>
    [Serializable]
    public sealed class ValueModifier : ValueEditor
    {
        public ValueSO valueSO;
        public ValueOperation operation;
        public float otherValue;

        public override void SetValue(IValueContext context)
        {
            float newValue = OperateNumericValue(context);
            ValueScope scope = context.GetValueScope(valueSO);
            context.DefineValue(valueSO, newValue, scope);
        }
        
        public float OperateNumericValue(IValueContext context)
        {
            float value = ValueUtility.GetNumericValue(valueSO, context);

            switch (operation)
            {
                case ValueOperation.Add: return value + otherValue;
                case ValueOperation.Subtract: return value - otherValue;
                case ValueOperation.Multiply: return value * otherValue;
                case ValueOperation.Divide: return value / otherValue;
                default: return 0;
            }
        }
    }
}