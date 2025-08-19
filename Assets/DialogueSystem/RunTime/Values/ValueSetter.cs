using System;
using System.ComponentModel;

namespace DialogueSystem.Runtime.Values
{
    [Serializable]
    public class ValueSetter<T> : ValueEditor
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