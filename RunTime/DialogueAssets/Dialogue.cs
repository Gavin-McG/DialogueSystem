using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// ScriptableObject representing a regular dialogue
    /// </summary>
    public sealed class Dialogue : DialogueTrace, IDialogueOutput
    {
        public DialogueTrace nextDialogue;
        [SerializeReference] public BaseParams baseParams;
        
        protected override DialogueTrace GetNextDialogue(AdvanceContext context, DialogueManager manager)
        {
            return nextDialogue;
        }

        public DialogueParams GetDialogueDetails(AdvanceContext context, DialogueManager manager)
        {
            return new DialogueParams(baseParams);
        }
    }

}