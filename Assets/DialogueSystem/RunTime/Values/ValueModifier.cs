using System;

namespace WolverineSoft.DialogueSystem.Runtime.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// ValueEditor which performs a mathematical operation on a numeric value
    /// </summary>
    [Serializable]
    public sealed class ValueModifier : ValueEditor
    {
        public string valueName;
        public ValueOperation operation;
        public float otherValue;

        public override void SetValue(IValueContext context)
        {
            float newValue = OperateNumericValue(context);
            ValueScope scope = context.GetValueScope(valueName);
            context.DefineValue(valueName, newValue, scope);
        }
        
        public float OperateNumericValue(IValueContext context)
        {
            float value = ValueUtility.GetNumericValue(valueName, context);

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