using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    [Serializable]
    public abstract class ValueHolder {}
    
    [Serializable]
    public class ValueHolder<T1> : ValueHolder
    {
        [SerializeReference] public T1 value1;
    }
    
    [Serializable]
    public class ValueHolder<T1, T2> : ValueHolder
    {
        [SerializeReference] public T1 value1;
        [SerializeReference] public T2 value2;
    }
    
    [Serializable]
    public class ValueHolder<T1, T2, T3> : ValueHolder
    {
        [SerializeReference] public T1 value1;
        [SerializeReference] public T2 value2;
        [SerializeReference] public T3 value3;
    }
}