using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    public abstract class OptionContextNode : ContextNode, IDialogueNode
    {
        public abstract bool UseText { get; }
        public abstract bool UseWeight { get; }

        public abstract ScriptableObject CreateDialogueObject();
        public abstract void AssignObjectReferences();
        public abstract DialogueObject GetData();
    }
}
