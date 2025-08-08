using System;

namespace DialogueSystem.Runtime
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DialoguePortAttribute : Attribute
    {
        public string DisplayName { get; }

        public DialoguePortAttribute() { }

        public DialoguePortAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
    
}