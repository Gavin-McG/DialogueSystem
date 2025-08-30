
namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Interface for <see cref="IDialogueReferenceNode"/> nodes that are also responsible assigning references to other scriptableObject on its own
    /// </summary>
    public interface IDialogueReferenceNode : IDialogueObjectNode
    {
        /// <summary>
        /// Method that is used to assign ScriptableObject references
        /// </summary>
        void AssignObjectReferences();
    }
}