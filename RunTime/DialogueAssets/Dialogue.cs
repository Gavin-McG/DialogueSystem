using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// ScriptableObject representing a regular dialogue
    /// </summary>
    public sealed class Dialogue : DialogueTrace, IDialogueOutput
    {
        [SerializeField] public DialogueTrace nextDialogue;
        [SerializeField] public TextData textData;
        
        protected override DialogueTrace GetNextDialogue(AdvanceParams advanceParams, DialogueManager manager)
        {
            return nextDialogue;
        }

        public DialogueParams GetDialogueDetails(AdvanceParams advanceParams, DialogueManager manager)
        {
            return new DialogueParams(textData);
        }
    }

}