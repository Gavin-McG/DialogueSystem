using System.Collections.Generic;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Runtime
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// ScriptableObject representing a regular dialogue
    /// </summary>
    public sealed class Dialogue : DialogueTrace, IDialogueOutput
    {
        public DialogueTrace nextDialogue;
        [SerializeReference] public BaseParams baseParams;
        
        protected override DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            return nextDialogue;
        }

        public DialogueParams GetDialogueDetails(AdvanceDialogueContext context, DialogueManager manager)
        {
            return new DialogueParams(baseParams);
        }
    }

}