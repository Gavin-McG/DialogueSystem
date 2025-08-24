using System;
using System.ComponentModel;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-24</date>
    
    /// <summary>
    /// Base class representing the parameters used by choice options
    /// </summary>
    [Serializable]
    public abstract class OptionParams : TextParams
    {
        public OptionParams() {}
        
        protected OptionParams(OptionParams other) : base(other) {}
        
        public abstract OptionParams Clone();
    }
}