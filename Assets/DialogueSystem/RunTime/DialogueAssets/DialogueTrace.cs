using System;
using System.Collections.Generic;
using DialogueSystem.Runtime.Keywords;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public abstract class DialogueTrace : ScriptableObject
    {
        [SerializeReference] public List<DSEventReference> events;
        public List<KeywordEditor> keywords;
        [SerializeReference] public List<Values.ValueEditor> values;
        
        public abstract DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager);

        public void RunOperations(DialogueManager manager)
        {
            InvokeEvents();
            ModifyKeywords(manager);
            ModifyValues(manager);
        }

        private void InvokeEvents()
        {
            foreach (var dialogueEvent in events)
            {
                dialogueEvent.Invoke();
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
