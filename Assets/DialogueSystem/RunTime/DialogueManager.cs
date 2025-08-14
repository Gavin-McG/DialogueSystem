using System;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem.Runtime
{

    public class DialogueManager : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<DialogueSettings> beginDialogueEvent = new();

        private DialogueAsset currentDialogue;
        private DialogueTrace currentTrace;

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
                return outputDialogue.GetDialogueDetails();
            
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
