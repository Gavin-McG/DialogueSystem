using System;

namespace WolverineSoft.DialogueSystem.Editor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TextHolderAttribute : Attribute
    {
        public int width;
        public int height;

        public TextHolderAttribute(int height, int width = -1)
        {
            this.height = height;
            this.width = width;
        }
    }
}