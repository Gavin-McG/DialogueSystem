using System;
using System.Collections.Generic;
using DialogueSystem.Runtime.Keywords;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Base class for ScriptableObjects that are part of the path traced by the <see cref="DialogueManager"/>
    /// </summary>
    public abstract class DialogueTrace : ScriptableObject
    {
        [SerializeField] public DialogueData data = new();

        public DialogueTrace AdvanceDialogue(AdvanceDialogueContext context, DialogueManager manager)
        {
            data.RunOperations(manager);
            return GetNextDialogue(context, manager);
        }
        
        protected abstract DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager);
    }

}
