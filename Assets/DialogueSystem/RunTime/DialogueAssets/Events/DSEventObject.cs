using UnityEngine;

namespace DialogueSystem.Runtime
{
    
    public abstract class DSEventObject : DialogueObject
    {
        public abstract void RemoveAllListeners();
    }
    
}