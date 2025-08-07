using System.Collections.Generic;
using DialogueSystem.Runtime;

namespace DialogueSystem.Editor
{
    public interface IDialogueTraceNode : IDialogueObjectNode
    {
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict);
    }
}