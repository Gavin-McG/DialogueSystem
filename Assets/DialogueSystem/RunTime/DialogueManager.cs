using System;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem.Runtime
{

    public class DialogueManager : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<DialogueSettings> beginDialogueEvent = new();
        
        private DialogueTrace currentDialogue;

        public void BeginDialogue(DialogueAsset dialogueAsset)
        {
            if (currentDialogue != null) return;
            currentDialogue = dialogueAsset;
            beginDialogueEvent.Invoke(dialogueAsset.settings);
        }

        public DialogueParams GetNextDialogue(AdvanceDialogueContext context)
        {
            do {
                currentDialogue.InvokeEvents();
                currentDialogue = currentDialogue.GetNextDialogue(context);
            } while (currentDialogue != null && currentDialogue is not IDialogueOutput);
            
            if (currentDialogue is IDialogueOutput outputDialogue)
                return outputDialogue.GetDialogueDetails();
            
            return null;
        }
        
        public DialogueParams GetNextDialogue() => GetNextDialogue(new  AdvanceDialogueContext());
    }
    

}
