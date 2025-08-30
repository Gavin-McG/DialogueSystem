using System;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class representing the parameters used by Choice Dialogue
    /// </summary>
    [Serializable]
    public abstract class ChoiceParams
    {
        public abstract ChoiceParams Clone();
    }
}