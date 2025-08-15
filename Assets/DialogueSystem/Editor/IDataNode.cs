using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public interface IDataNode<out T>
    {
        public T GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict);
    }
}