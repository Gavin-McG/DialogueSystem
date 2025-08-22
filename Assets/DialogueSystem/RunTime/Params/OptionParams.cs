using System;
using System.ComponentModel;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Base class representing the parameters used by choice options
    /// </summary>
    [Serializable]
    public abstract class OptionParams
    {
        public string text;
        
        public string Text { get => text; set => text = value; }
        
        public abstract OptionParams Clone();
    }
}