using System;
using System.ComponentModel;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-24</date>
    
    /// <summary>
    /// ValueEditor representing the assignment of a value
    /// </summary>
    [Serializable]
    public sealed class ValueSetter<T> : ValueEditor
    {
        public ValueSO valueSO;

        [DefaultValue(ValueSO.ValueScope.Dialogue)]
        public ValueSO.ValueScope scope;
        
        public T value;

        public override void SetValue(IValueContext context)
        {
            valueSO.SetValue(context, scope, value);
        }
    }
}