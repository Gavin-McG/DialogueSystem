using DialogueSystem.Runtime;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class DSEventNodeString : DSEventNode<string>
    {
        public override DialogueObject CreateDialogueObject()
        {
            var stringEventReference = ScriptableObject.CreateInstance<DSEventReferenceString>();
            AssignEventReferenceValues(stringEventReference);
            
            return stringEventReference;
        }
    }
}