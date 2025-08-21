using System;
using System.Collections.Generic;
using DialogueSystem.Runtime.Keywords;
using DialogueSystem.Runtime.Values;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class DialogueData
    {
        [SerializeReference] public List<DSEventReference> events;
        public List<KeywordEditor> keywords;
        [SerializeReference] public List<ValueEditor> values;
        
        public void RunOperations(DialogueManager manager)
        {
            ModifyKeywords(manager);
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

        private void ModifyKeywords(DialogueManager manager)
        {
            foreach (var entry in keywords)
            {
                entry.Apply(manager);
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