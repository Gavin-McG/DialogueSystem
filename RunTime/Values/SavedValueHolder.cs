using System.Collections.Generic;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// Serialization-friendly format for <see cref="ValueHolder"/> data
    /// </summary>
    [System.Serializable]
    public class SavedValueHolder
    {
        public List<SavedValueEntry> Values = new();
    }
}