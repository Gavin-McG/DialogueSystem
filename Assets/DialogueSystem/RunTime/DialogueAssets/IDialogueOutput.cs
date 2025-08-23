using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Interface for DialogueTrace classes which are meant to provide an output to AdvanceDialogue
    /// Currently limited to <see cref="Dialogue"/> and <see cref="ChoiceDialogue"/>
    /// </summary>
    public interface IDialogueOutput
    {
        public DialogueParams GetDialogueDetails(AdvanceDialogueContext context, DialogueManager manager);
    }

}
