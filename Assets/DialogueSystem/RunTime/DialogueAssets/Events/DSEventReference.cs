using UnityEngine;

namespace DialogueSystem.Runtime
{
    public class DSEventReference<T> : DSEventCaller
    {
        [SerializeField, HideInDialogueGraph] public DSEvent<T> dialogueEvent;
        [SerializeField] public T value;

        public override void Invoke()
        {
            dialogueEvent.Invoke(value);
        }

        public override void RemoveAllListeners()
        {
            dialogueEvent.RemoveAllListeners();
        }
    }
}