using System;
using UnityEngine.Serialization;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// ValueEditor which performs a mathematical operation on a numeric value
    /// </summary>
    [Serializable]
    public sealed class ValueModifier : ValueEditor
    {
        [FormerlySerializedAs("valueSO")] public DSValue dsValue;
        public DSValue.ValueOperation operation;
        public float otherValue;

        public override void SetValue(IValueContext context)
        {
            dsValue.TryOperateValue(context, this.operation, this.otherValue);
        }
    }
}