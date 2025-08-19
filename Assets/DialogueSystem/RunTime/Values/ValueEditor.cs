using System;

namespace DialogueSystem.Runtime.Values
{
    [Serializable]
    public abstract class ValueEditor
    {
        public abstract void SetValue(IValueContext context);
    }
}