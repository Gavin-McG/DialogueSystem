using System;

namespace DialogueSystem.Runtime.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Base class for objects representing a modification to a IValueContext
    /// </summary>
    [Serializable]
    public abstract class ValueEditor
    {
        public abstract void SetValue(IValueContext context);
    }
}