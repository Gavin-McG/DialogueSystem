using System;
using System.Collections.Generic;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// Serialization-friendly format for <see cref="DSValueHolder"/> data.
    /// Requires a form of serialization which supports polymorphism, such as NewtonSoft
    /// </summary>
    [Serializable]
    public class SavedDSValueHolder
    {
        public List<SavedDSValue> Values = new();
    }
}