using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class for <see cref="EventCaller"/> and <see cref="EventCaller{T}"/>
    /// </summary>
    [Serializable]
    public abstract class EventReference
    {
        [SerializeField] public DSEventBase dialogueEvent;
        
        public abstract void Invoke();
        
        public abstract void Invoke(DialogueManager manager);
    }
}