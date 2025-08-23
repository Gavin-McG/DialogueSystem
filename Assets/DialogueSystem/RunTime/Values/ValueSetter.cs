using System;
using System.ComponentModel;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// ValueEditor representing the assignment of a value
    /// </summary>
    [Serializable]
    public sealed class ValueSetter<T> : ValueEditor
    {
        public ValueSO valueSO;

        [DefaultValue(ValueScope.Dialogue)]
        public ValueScope scope;
        
        public T value;

        public override void SetValue(IValueContext context)
        {
            context.DefineValue(valueSO, value, scope);
        }
    }
}