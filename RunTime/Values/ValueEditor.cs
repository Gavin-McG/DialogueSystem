using System;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// Base class for objects representing a modification to a IValueContext
    /// </summary>
    [Serializable]
    public abstract class ValueEditor
    {
        public abstract void SetValue(IValueContext context);
    }
}