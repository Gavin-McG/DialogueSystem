using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// Serialization-friendly format for <see cref="DSValue"/> data. 
    /// Requires a form of serialization which supports polymorphism, such as NewtonSoft
    /// </summary>
    [Serializable]
    public class SavedValueEntry
    {
        public string ValueId;
        [JsonProperty] internal List<ValueInstance> Instances = new List<ValueInstance>();
    }
}