using System.Collections.Generic;

namespace WolverineSoft.DialogueSystem
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Base class for redirect objects which select the next DialogueTrace based on ConditionalOptions
    /// </summary>
    public abstract class Redirect : DialogueTrace
    {
        public DialogueTrace defaultDialogue;
        public List<Option> options;
    }
}