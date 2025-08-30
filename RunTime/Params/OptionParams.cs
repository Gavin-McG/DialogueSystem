using System;

namespace WolverineSoft.DialogueSystem
{
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