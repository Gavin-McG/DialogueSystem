using System;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// class Holding information for a call to a non-types DSEvent
    /// </summary>
    [Serializable]
    public class EventCaller : EventReference
    {
        private DSEvent Event => dialogueEvent as DSEvent;

        public override void Invoke()
        {
            Event?.Invoke();
        }

        public override void Invoke(DialogueManager manager)
        {
            Event?.Invoke(manager);
        }
    }

    /// <summary>
    /// class Holding information for a call to a non-types DSEvent
    /// </summary>
    [Serializable]
    public class EventCaller<T> : EventReference
    {
        public T value;
        
        private DSEvent<T> Event => dialogueEvent as DSEvent<T>;
        
        public override void Invoke()
        {
            Event?.Invoke(value);
        }

        public override void Invoke(DialogueManager manager)
        {
            Event?.Invoke(manager, value);
        }
    }
}