using System;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class representing the parameters used by Choice Dialogue
    /// </summary>
    public class ChoiceParams
    {
        //include time limit by default
        public bool hasTimeLimit;
        public float timeLimitDuration;
    }
}