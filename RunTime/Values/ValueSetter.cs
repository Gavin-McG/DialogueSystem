using System;
using System.ComponentModel;
using UnityEngine.Serialization;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// ValueEditor representing the assignment of a value
    /// </summary>
    [Serializable]
    public sealed class ValueSetter<T> : ValueEditor
    {
        public DSValue dsValue;

        [DefaultValue(DSValue.ValueScope.Dialogue)]
        public DSValue.ValueScope scope;
        
        public T value;

        public override void SetValue(IValueContext context)
        {
            dsValue.SetValue(context, scope, value);
        }
    }
}