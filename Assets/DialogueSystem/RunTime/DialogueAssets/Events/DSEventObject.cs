using UnityEngine;

namespace DialogueSystem.Runtime
{
    
    public abstract class DSEventObject : ScriptableObject
    {
        public abstract void RemoveAllListeners();
    }
    
}