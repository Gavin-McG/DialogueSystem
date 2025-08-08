using System;
using DialogueSystem.Runtime;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class DSEventNodeInt : DSEventNode<int>
    {
        public override DialogueObject CreateDialogueObject()
        {
            var intEventReference = ScriptableObject.CreateInstance<DSEventReferenceInt>();
            AssignEventReferenceValues(intEventReference);
            
            return intEventReference;
        }
    }
}