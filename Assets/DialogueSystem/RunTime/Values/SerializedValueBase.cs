using System;

namespace WolverineSoft.DialogueSystem.Values
{
    [Serializable]
    public abstract class SerializedValueBase
    {
        public abstract object GetValue();
    }

    [Serializable]
    public class SerializedValue<T> : SerializedValueBase
    {
        public T Value;
        public SerializedValue() => Value = default;
        public SerializedValue(T value = default) => Value = value;
        
        public override object GetValue() => Value;
    }
}