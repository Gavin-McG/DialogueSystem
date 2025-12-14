using System;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class for <see cref="EventCaller"/> and <see cref="EventCaller{T}"/>
    /// </summary>
    [Serializable]
    public abstract class EventReference
    {
        public abstract void Invoke();
        
        public abstract void Invoke(DialogueManager manager);
    }
}