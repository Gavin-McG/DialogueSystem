using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem.Runtime
{
    [CreateAssetMenu(fileName = "DialogueEvent", menuName = "Dialogue System/Events/Dialogue Event")]
    public class DSEvent : DSEventCaller
    {
        [SerializeField] private UnityEvent dialogueEvent = new UnityEvent();

        public override void Invoke()
        {
            dialogueEvent.Invoke();
        }
        
        public void AddListener(UnityAction call)
        {
            dialogueEvent.AddListener(call);
        }

        public void RemoveListener(UnityAction call)
        {
            dialogueEvent.RemoveListener(call);
        }

        public override void RemoveAllListeners()
        {
            dialogueEvent.RemoveAllListeners();
        }
    }

    public abstract class DSEvent<T> : DSEventObject
    {
        [SerializeField] private UnityEvent<T> dialogueEvent = new UnityEvent<T>();

        public void Invoke(T value)
        {
            dialogueEvent.Invoke(value);
        }

        public void AddListener(UnityAction<T> call)
        {
            dialogueEvent.AddListener(call);
        }

        public void RemoveListener(UnityAction<T> call)
        {
            dialogueEvent.RemoveListener(call);
        }

        public override void RemoveAllListeners()
        {
            dialogueEvent.RemoveAllListeners();
        }
    }

}
