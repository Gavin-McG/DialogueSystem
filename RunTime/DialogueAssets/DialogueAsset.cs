using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// ScriptableObject representing the main asset of a DialogueGraph and the first Trace when starting a dialogue
    /// </summary>
    public sealed class DialogueAsset : DialogueTrace
    {
        public DialogueTrace nextDialogue;
        [SerializeReference] public DialogueSettings settings;
        public DialogueData endData = new();
        
        protected override DialogueTrace GetNextDialogue(AdvanceParams advanceParams, DialogueManager manager)
        {
            return nextDialogue;
        }

        public void RunEndOperations(DialogueManager manager)
        {
            endData.RunOperations(manager);
        }
    }

}
