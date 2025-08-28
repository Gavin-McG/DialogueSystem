using System;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-24</date>
    
    /// <summary>
    /// ValueEditor which performs a mathematical operation on a numeric value
    /// </summary>
    [Serializable]
    public sealed class ValueModifier : ValueEditor
    {
        public ValueSO valueSO;
        public ValueSO.ValueOperation operation;
        public float otherValue;

        public override void SetValue(IValueContext context)
        {
            valueSO.TryOperateValue(context, this.operation, this.otherValue);
        }
    }
}