using System;
using System.Collections.Generic;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
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
