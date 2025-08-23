using UnityEngine;

namespace WolverineSoft.DialogueSystem.Runtime
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Base ScriptableObject class for <see cref="DSEvent{T}"/> and <see cref="DSEvent"/>
    /// </summary>
    public abstract class DSEventObject : ScriptableObject
    {
        public abstract void RemoveAllListeners();
    }
    
}