using System;

namespace WolverineSoft.DialogueSystem
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TypeOptionAttribute : Attribute
    {
        public string displayName;
        public int order;

        public TypeOptionAttribute(string displayName, int order = 0)
        {
            this.displayName = displayName;
            this.order = order;
        }
    }
}