using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem.Runtime
{

    public class DialogueManager : MonoBehaviour
    {
        private readonly Dictionary<string, object> insertValues = new();
        
        [HideInInspector] public UnityEvent<DialogueSettings> beginDialogueEvent = new();

        private DialogueAsset currentDialogue;
        private DialogueTrace currentTrace;

        public void SetValue(string valueName, object value)
        {
            insertValues[valueName] = value;
        }

        public object GetValue(string valueName)
        {
            if (insertValues.TryGetValue(valueName, out var value))
                return value;

            return $"{{No Value \"{valueName}\"}}";
        }
        
        public string ReplaceValues(string input)
        {
            return Regex.Replace(input, @"\{(.*?)\}", match =>
            {
                string key = match.Groups[1].Value;
                object value = GetValue(key);
                return value?.ToString() ?? "";
            });
        }

        public void BeginDialogue(DialogueAsset dialogueAsset)
        {
            if (currentDialogue != null)
            {
                Debug.LogWarning($"Attempted to begin dialogue \"{dialogueAsset.name}\" while dialogue was already playing");
                return;
            }
            
            currentDialogue = dialogueAsset;
            currentTrace = dialogueAsset;
            beginDialogueEvent.Invoke(dialogueAsset.settings);
        }

        public DialogueParams GetNextDialogue(AdvanceDialogueContext context)
        {
            do {
                currentTrace.InvokeEvents();
                currentTrace = currentTrace.GetNextDialogue(context);
            } while (currentTrace != null && currentTrace is not IDialogueOutput);

            if (currentTrace is IDialogueOutput outputDialogue)
            {
                var details = new DialogueParams(outputDialogue.GetDialogueDetails());
                Debug.Log(details.baseParams.text);
                details.baseParams.Text = ReplaceValues(details.baseParams.Text);
                return details;
            }
            
            EndDialogue();
            return null;
        }
        
        public DialogueParams GetNextDialogue() => GetNextDialogue(new AdvanceDialogueContext());

        public void EndDialogue()
        {
            if (currentDialogue == null) return;
            
            currentDialogue.InvokeEndEvents();
            
            currentDialogue = null;
            currentTrace = null;
        }
    }
    

}
