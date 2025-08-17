using System;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class DSEventCaller : DSEventReference
    {
        public DSEvent dialogueEvent;

        public override void Invoke()
        {
            dialogueEvent.Invoke();
        }
    }

    [Serializable]
    public class DSEventCaller<T> : DSEventReference
    {
        public DSEvent<T> dialogueEvent;
        public T value;

        public override void Invoke()
        {
            dialogueEvent.Invoke(value);
        }
    }
}