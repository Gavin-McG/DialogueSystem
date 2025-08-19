using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem.Runtime
{
    [CreateAssetMenu(fileName = "DialogueEvent", menuName = "Dialogue System/Events/Dialogue Event")]
    public class DSEvent : DSEventObject
    {
        [SerializeField] private UnityEvent dialogueEvent = new UnityEvent();

        // List of manager-specific listeners
        private readonly Dictionary<DialogueManager, UnityEvent> managerEvents = new Dictionary<DialogueManager, UnityEvent>();

        public void Invoke()
        {
            // Invoke general listeners
            dialogueEvent.Invoke();

            // Invoke all manager-specific listeners
            foreach (var evt in managerEvents.Values)
                evt.Invoke();
        }

        public void Invoke(DialogueManager manager)
        {
            // Always invoke general listeners
            dialogueEvent.Invoke();

            // Only invoke listeners registered with this manager
            if (manager != null && managerEvents.TryGetValue(manager, out var evt))
                evt.Invoke();
        }

        public void AddListener(UnityAction call)
        {
            dialogueEvent.AddListener(call);
        }

        public void AddListener(DialogueManager manager, UnityAction call)
        {
            if (!managerEvents.TryGetValue(manager, out var evt))
            {
                evt = new UnityEvent();
                managerEvents[manager] = evt;
            }
            evt.AddListener(call);
        }

        public void RemoveListener(UnityAction call)
        {
            dialogueEvent.RemoveListener(call);
        }

        public void RemoveListener(DialogueManager manager, UnityAction call)
        {
            if (managerEvents.TryGetValue(manager, out var evt))
            {
                evt.RemoveListener(call);
            }
        }

        public override void RemoveAllListeners()
        {
            dialogueEvent.RemoveAllListeners();
            managerEvents.Clear();
        }
    }

    public abstract class DSEvent<T> : DSEventObject
    {
        [SerializeField] private UnityEvent<T> dialogueEvent = new UnityEvent<T>();

        private readonly Dictionary<DialogueManager, UnityEvent<T>> managerEvents = new Dictionary<DialogueManager, UnityEvent<T>>();

        public void Invoke(T value)
        {
            dialogueEvent.Invoke(value);

            foreach (var evt in managerEvents.Values)
                evt.Invoke(value);
        }

        public void Invoke(DialogueManager manager, T value)
        {
            // Always invoke general listeners
            dialogueEvent.Invoke(value);

            if (manager != null && managerEvents.TryGetValue(manager, out var evt))
                evt.Invoke(value);
        }

        public void AddListener(UnityAction<T> call)
        {
            dialogueEvent.AddListener(call);
        }

        public void AddListener(DialogueManager manager, UnityAction<T> call)
        {
            if (!managerEvents.TryGetValue(manager, out var evt))
            {
                evt = new UnityEvent<T>();
                managerEvents[manager] = evt;
            }
            evt.AddListener(call);
        }

        public void RemoveListener(UnityAction<T> call)
        {
            dialogueEvent.RemoveListener(call);
        }

        public void RemoveListener(DialogueManager manager, UnityAction<T> call)
        {
            if (managerEvents.TryGetValue(manager, out var evt))
            {
                evt.RemoveListener(call);
            }
        }

        public override void RemoveAllListeners()
        {
            dialogueEvent.RemoveAllListeners();
            managerEvents.Clear();
        }
    }
}
