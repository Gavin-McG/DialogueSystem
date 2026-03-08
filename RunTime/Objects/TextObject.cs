using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// ScriptableObject representing a regular dialogue
    /// </summary>
    public sealed class TextObject : DialogueObject, IDialogueOutput
    {
        [SerializeField] public DialogueObject nextDialogue;
        [SerializeField] public string text;
        [SerializeReference] public TextParameters textParams;
        
        public override DialogueObject GetNextDialogue(AdvanceContext advanceContext, DialogueManager manager)
        {
            return nextDialogue;
        }

        public DialogueInfo GetDialogueDetails(AdvanceContext advanceContext, DialogueManager manager)
        {
            return new DialogueInfo(text, textParams);
        }
    }

}