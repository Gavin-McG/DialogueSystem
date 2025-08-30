using System;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// class Holding information for a call to a non-types DSEvent
    /// </summary>
    [Serializable]
    public class DSEventCaller : DSEventReference
    {
        public DSEvent dialogueEvent;
        
        public override void Invoke()
        {
            dialogueEvent.Invoke();
        }

        public override void Invoke(DialogueManager manager)
        {
            dialogueEvent.Invoke(manager);
        }
    }

    /// <summary>
    /// class Holding information for a call to a non-types DSEvent
    /// </summary>
    [Serializable]
    public class DSEventCaller<T> : DSEventReference
    {
        public DSEvent<T> dialogueEvent;
        public T value;
        
        public override void Invoke()
        {
            dialogueEvent.Invoke(value);
        }

        public override void Invoke(DialogueManager manager)
        {
            dialogueEvent.Invoke(manager, value);
        }
    }
}