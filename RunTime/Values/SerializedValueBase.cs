using System;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// Base class for preserving object data through Unity's serialization
    /// </summary>
    [Serializable]
    public abstract class SerializedValueBase
    {
        public abstract object GetValue();
    }

    /// <summary>
    /// Class for preserving object data through Unity's serialization
    /// </summary>
    [Serializable]
    public class SerializedValue<T> : SerializedValueBase
    {
        public T Value;
        public SerializedValue() => Value = default;
        public SerializedValue(T value = default) => Value = value;
        
        public override object GetValue() => Value;
    }
}