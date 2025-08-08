using System;
using DialogueSystem.Runtime;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class DSEventNodeVector3 : DSEventNode<Vector3>
    {
        public override DialogueObject CreateDialogueObject()
        {
            var vector3EventReference = ScriptableObject.CreateInstance<DSEventReferenceVector3>();
            AssignEventReferenceValues(vector3EventReference);
            
            return vector3EventReference;
        }
    }
}