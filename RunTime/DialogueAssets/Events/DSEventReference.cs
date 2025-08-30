using System;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class for <see cref="DSEventCaller"/> and <see cref="DSEventCaller{T}"/>
    /// </summary>
    [Serializable]
    public abstract class DSEventReference
    {
        public abstract void Invoke();
        
        public abstract void Invoke(DialogueManager manager);
    }
}