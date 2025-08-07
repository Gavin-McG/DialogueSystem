using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem.Runtime
{
    [CreateAssetMenu(fileName = "DialogueEvent", menuName = "Dialogue System/Dialogue Event")]
    public class DialogueEvent : DialogueObject
    {
        public UnityEvent dialogueEvent = new UnityEvent();
    }

}
