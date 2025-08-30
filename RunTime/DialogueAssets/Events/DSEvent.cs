using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Scriptable Object representing a non-typed Dialogue Event
    /// </summary>
    [CreateAssetMenu(fileName = "DialogueEvent", menuName = "Dialogue System/Events/Dialogue Event")]
    public sealed class DSEvent : DSEventObject
    {
        [SerializeField] private UnityEvent dialogueEvent = new UnityEvent();

        // List of manager-specific listeners
        private readonly Dictionary<DialogueManager, UnityEvent> managerEvents = new Dictionary<DialogueManager, UnityEvent>();

        /// <summary>
        /// Invoke Event for all listeners
        /// </summary>
        public void Invoke()
        {
            // Invoke general listeners
            dialogueEvent.Invoke();

            // Invoke all manager-specific listeners
            foreach (var evt in managerEvents.Values)
                evt.Invoke();
        }

        /// <summary>
        /// Invoke Event for all global listeners.
        /// Listeners assigned with a manager will only be called if the provided manager matches
        /// </summary>
        public void Invoke(DialogueManager manager)
        {
            // Always invoke general listeners
            dialogueEvent.Invoke();

            // Only invoke listeners registered with this manager
            if (manager != null && managerEvents.TryGetValue(manager, out var evt))
                evt.Invoke();
        }

        /// <summary>
        /// Add a global listener
        /// </summary>
        public void AddListener(UnityAction call)
        {
            dialogueEvent.AddListener(call);
        }

        /// <summary>
        /// Add a manager-specific listener
        /// </summary>
        public void AddListener(DialogueManager manager, UnityAction call)
        {
            if (!managerEvents.TryGetValue(manager, out var evt))
            {
                evt = new UnityEvent();
                managerEvents[manager] = evt;
            }
            evt.AddListener(call);
        }

        /// <summary>
        /// Remove a Global listener
        /// </summary>
        public void RemoveListener(UnityAction call)
        {
            dialogueEvent.RemoveListener(call);
        }

        /// <summary>
        /// Remove a manager-specific listener
        /// </summary>
        public void RemoveListener(DialogueManager manager, UnityAction call)
        {
            if (managerEvents.TryGetValue(manager, out var evt))
            {
                evt.RemoveListener(call);
            }
        }

        /// <summary>
        /// Remove all global and manager-specific listeners
        /// </summary>
        public override void RemoveAllListeners()
        {
            dialogueEvent.RemoveAllListeners();
            managerEvents.Clear();
        }
    }

    /// <summary>
    /// abstract class for Scriptable Object representing a typed Dialogue Event
    /// </summary>
    public abstract class DSEvent<T> : DSEventObject
    {
        [SerializeField] private UnityEvent<T> dialogueEvent = new UnityEvent<T>();

        private readonly Dictionary<DialogueManager, UnityEvent<T>> managerEvents = new Dictionary<DialogueManager, UnityEvent<T>>();

        /// <summary>
        /// Invoke Event for all listeners
        /// </summary>
        public void Invoke(T value)
        {
            dialogueEvent.Invoke(value);

            foreach (var evt in managerEvents.Values)
                evt.Invoke(value);
        }

        /// <summary>
        /// Invoke Event for all global listeners.
        /// Listeners assigned with a manager will only be called if the provided manager matches
        /// </summary>
        public void Invoke(DialogueManager manager, T value)
        {
            // Always invoke general listeners
            dialogueEvent.Invoke(value);

            if (manager != null && managerEvents.TryGetValue(manager, out var evt))
                evt.Invoke(value);
        }

        /// <summary>
        /// Add a global listener
        /// </summary>
        public void AddListener(UnityAction<T> call)
        {
            dialogueEvent.AddListener(call);
        }

        /// <summary>
        /// Add a manager-specific listener
        /// </summary>
        public void AddListener(DialogueManager manager, UnityAction<T> call)
        {
            if (!managerEvents.TryGetValue(manager, out var evt))
            {
                evt = new UnityEvent<T>();
                managerEvents[manager] = evt;
            }
            evt.AddListener(call);
        }

        /// <summary>
        /// Remove a Global listener
        /// </summary>
        public void RemoveListener(UnityAction<T> call)
        {
            dialogueEvent.RemoveListener(call);
        }

        /// <summary>
        /// Remove a manager-specific listener
        /// </summary>
        public void RemoveListener(DialogueManager manager, UnityAction<T> call)
        {
            if (managerEvents.TryGetValue(manager, out var evt))
            {
                evt.RemoveListener(call);
            }
        }

        /// <summary>
        /// Remove all global and manager-specific listeners
        /// </summary>
        public override void RemoveAllListeners()
        {
            dialogueEvent.RemoveAllListeners();
            managerEvents.Clear();
        }
    }
}
