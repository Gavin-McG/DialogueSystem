using System;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Attribute used to denote that a specific field in a parameter class should be added as a port rather than an option
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class DialoguePortAttribute : Attribute
    {
        
    }
}