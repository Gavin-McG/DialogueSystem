using System;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Attribute used to mark fields which should not be displays in Node inspectors
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class HideInDialogueGraphAttribute : Attribute
    {
        
    }
}