using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public abstract class DialogueTrace : ScriptableObject
    {
        [SerializeReference] public List<DSEventReference> events;
        public List<Keywords.KeywordEntry> keywords;
        public List<ValueSetter> values;
        
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
                switch (entry.operation)
                {
                    case Keywords.Operation.Add: manager.AddKeyword(entry.keyword); break;
                    case Keywords.Operation.Remove: manager.RemoveKeyword(entry.keyword); break;
                    case Keywords.Operation.RemoveAll: manager.ClearKeywords(); break;
                    default: break;
                }
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
