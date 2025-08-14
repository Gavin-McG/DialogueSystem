using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public abstract class DialogueTrace : ScriptableObject
    {
        public List<DSEventCaller> events;
        public List<KeywordEntry> keywords;

        [Serializable]
        public class KeywordEntry
        {
            public string keyword;
            public Operation operation;
                
            public enum Operation { Add, Remove, RemoveAll }
        }
        
        public abstract DialogueTrace GetNextDialogue(AdvanceDialogueContext context, DialogueManager manager);

        public void InvokeEvents()
        {
            foreach (var dialogueEvent in events)
            {
                dialogueEvent.Invoke();
            }
        }

        public void ModifyKeywords(DialogueManager manager)
        {
            foreach (var entry in keywords)
            {
                switch (entry.operation)
                {
                    case KeywordEntry.Operation.Add: manager.AddKeyword(entry.keyword); break;
                    case KeywordEntry.Operation.Remove: manager.RemoveKeyword(entry.keyword); break;
                    case KeywordEntry.Operation.RemoveAll: manager.ClearKeywords(); break;
                    default: break;
                }
            }
        }
    }

}
