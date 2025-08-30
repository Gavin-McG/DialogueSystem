using System;
using System.Collections.Generic;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// Serialization-friendly format for <see cref="DSValue"/> data
    /// </summary>
    [Serializable]
    public struct SavedValueEntry
    {
        public string ValueId;
        internal List<ValueInstance> Instances;
    }
}