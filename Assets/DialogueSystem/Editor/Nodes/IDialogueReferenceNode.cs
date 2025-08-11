using System.Collections.Generic;
using DialogueSystem.Runtime;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public interface IDialogueReferenceNode : IDialogueObjectNode
    {
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict);
    }
}