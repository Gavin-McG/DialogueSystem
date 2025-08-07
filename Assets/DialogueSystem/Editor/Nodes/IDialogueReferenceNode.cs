using System.Collections.Generic;
using DialogueSystem.Runtime;

namespace DialogueSystem.Editor
{
    public interface IDialogueReferenceNode : IDialogueObjectNode
    {
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict);
    }
}