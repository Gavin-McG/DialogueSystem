using System;
using System.Collections.Generic;
using DialogueSystem.Runtime.Keywords;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class DialogueData
    {
        [SerializeReference] public List<DSEventReference> events;
        public List<KeywordEditor> keywords;
        [SerializeReference] public List<Values.ValueEditor> values;
        
        public void RunOperations(DialogueManager manager)
        {
            InvokeEvents(manager);
            ModifyKeywords(manager);
            ModifyValues(manager);
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