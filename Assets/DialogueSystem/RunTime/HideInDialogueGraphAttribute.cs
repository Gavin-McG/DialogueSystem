using System;

namespace WolverineSoft.DialogueSystem.Runtime
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Attribute used to mark fields which should not be displays in Node inspectors
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class HideInDialogueGraphAttribute : Attribute
    {
        
    }
}