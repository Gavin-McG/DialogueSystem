using UnityEngine;


namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Interface for nodes that are responsible for creating a scriptableObject instance
    /// </summary>
    public interface IDialogueObjectNode
    {
        /// <summary>
        /// Method that create and returns the node's scriptableObject
        /// </summary>
        ScriptableObject CreateDialogueObject();
    }

}
