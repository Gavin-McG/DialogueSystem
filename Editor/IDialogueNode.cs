using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    public interface IDialogueNode : IDataNode<DialogueObject>
    {
        /// <summary>
        /// Method that creates and returns the node's ScriptableObject
        /// </summary>
        ScriptableObject CreateDialogueObject();
        
        /// <summary>
        /// Method that is used to assign ScriptableObject references
        /// </summary>
        void AssignObjectReferences();

        void CheckErrors(GraphLogger logger, IVariableContext variables) { }
    }
}