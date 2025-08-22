using System;
using System.ComponentModel;

namespace DialogueSystem.Runtime.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// ValueEditor representing the assignment of a value
    /// </summary>
    [Serializable]
    public sealed class ValueSetter<T> : ValueEditor
    {
        [DefaultValue("MyValue")]
        public string valueName;

        [DefaultValue(ValueScope.Dialogue)]
        public ValueScope scope;
        
        public T value;

        public override void SetValue(IValueContext context)
        {
            context.DefineValue(valueName, value, scope);
        }
    }
}