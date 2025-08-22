using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// ScriptableObject representing the main asset of a DialogueGraph and the first Trace when starting a dialogue
    /// </summary>
    public sealed class DialogueAsset : DialogueTrace
    {
        public DialogueTrace nextDialogue;
        public DialogueSettings settings;
        public DialogueData endData = new();
        
        protected override DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            return nextDialogue;
        }

        public void RunEndOperations(DialogueManager manager)
        {
            endData.RunOperations(manager);
        }
    }

}
