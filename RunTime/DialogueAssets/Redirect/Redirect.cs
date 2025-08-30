using System.Collections.Generic;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class for redirect objects which select the next DialogueTrace based on ConditionalOptions
    /// </summary>
    public abstract class Redirect : DialogueTrace
    {
        public DialogueTrace defaultDialogue;
        public List<Option> options;
    }
}