
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    public class EventObject : DialogueObject
    {
        [SerializeField] public DialogueObject nextDialogue;
        [SerializeReference] public EventReference eventCaller;

        public override DialogueObject GetNextDialogue(AdvanceContext context, DialogueManager manager)
        {
            eventCaller.Invoke();
            return nextDialogue;
        }
    }
}
