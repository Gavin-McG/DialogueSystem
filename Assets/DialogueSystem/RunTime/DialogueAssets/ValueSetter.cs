using UnityEngine;

namespace DialogueSystem.Runtime
{
    public abstract class ValueSetter : ScriptableObject
    {
        public abstract void SetValue(DialogueManager manager);
    }
    
    public abstract class ValueSetter<T> : ValueSetter
    {
        public string valueName;
        public T value;

        public override void SetValue(DialogueManager manager)
        {
            manager.SetValue(valueName, value);
        }
    }
}