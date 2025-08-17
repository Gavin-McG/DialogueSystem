using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public abstract class DSEventReference
    {
        public abstract void Invoke();
    }
}