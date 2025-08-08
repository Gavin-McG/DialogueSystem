using System;
using DialogueSystem.Runtime;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class DSEventNodeShirt : DSEventNode<Shirt>
    {
        public override DialogueObject CreateDialogueObject()
        {
            var shirtEventReference = ScriptableObject.CreateInstance<DSEventReferenceShirt>();
            AssignEventReferenceValues(shirtEventReference);
            
            return shirtEventReference;
        }
    }
}