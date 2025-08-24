using System;
using System.Collections.Generic;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Stores all the data about event, keyword, and value operations which are to occur after a given DialogueTrace
    /// Events are called last in case any values or keywords might affect be required for that event
    /// </summary>
    [Serializable]
    public class DialogueData
    {
        [SerializeReference] public List<DSEventReference> events;
        [SerializeReference] public List<ValueEditor> values;
        
        //Applies all changes that this instance represents
        public void RunOperations(DialogueManager manager)
        {
            ModifyValues(manager);
            InvokeEvents(manager);
        }

        private void InvokeEvents(DialogueManager manager)
        {
            foreach (var dialogueEvent in events)
            {
                dialogueEvent.Invoke(manager);
            }
        }

        private void ModifyValues(DialogueManager manager)
        {
            foreach (var entry in values)
            {
                entry.SetValue(manager);
            }
        }
    }
}