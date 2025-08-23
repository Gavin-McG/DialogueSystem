using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Runtime
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
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