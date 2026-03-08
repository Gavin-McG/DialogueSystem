using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    [Serializable]
    public abstract class GenericValueHolder {}
    
    [Serializable]
    public class GenericValueHolder<T1> : GenericValueHolder
    {
        [SerializeReference] public T1 value1;
    }
    
    [Serializable]
    public class GenericValueHolder<T1, T2> : GenericValueHolder
    {
        [SerializeReference] public T1 value1;
        [SerializeReference] public T2 value2;
    }
    
    [Serializable]
    public class GenericValueHolder<T1, T2, T3> : GenericValueHolder
    {
        [SerializeReference] public T1 value1;
        [SerializeReference] public T2 value2;
        [SerializeReference] public T3 value3;
    }
}