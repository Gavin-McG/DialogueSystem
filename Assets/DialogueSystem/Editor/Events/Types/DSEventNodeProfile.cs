using System;
using DialogueSystem.Runtime;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class DSEventNodeProfile : DSEventNode<DialogueProfile>
    {
        public override DialogueObject CreateDialogueObject()
        {
            var profileEventReference = ScriptableObject.CreateInstance<DSEventReferenceProfile>();
            AssignEventReferenceValues(profileEventReference);
            
            return profileEventReference;
        }
    }
}