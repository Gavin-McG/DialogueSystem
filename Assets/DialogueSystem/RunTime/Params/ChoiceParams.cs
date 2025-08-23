using System;

namespace WolverineSoft.DialogueSystem
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Base class representing the parameters used by Choice Dialogue
    /// </summary>
    [Serializable]
    public abstract class ChoiceParams
    {
        public abstract ChoiceParams Clone();
    }
}