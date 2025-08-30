using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base ScriptableObject class for <see cref="DSEvent{T}"/> and <see cref="DSEvent"/>
    /// </summary>
    public abstract class DSEventObject : ScriptableObject
    {
        public abstract void RemoveAllListeners();
    }
    
}