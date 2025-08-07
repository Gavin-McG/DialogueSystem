using System;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem.Runtime
{

    public class DialogueManager : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<DialogueAsset> beginDialogue = new();
        [HideInInspector] public UnityEvent<AdvanceDialogueContext> advanceDialogue = new();
        public UnityEvent<DialogueParams> displayDialogue = new();
        public UnityEvent endDialogue = new();
        
        private DialogueTrace currentDialogue;

        private void OnEnable()
        {
            beginDialogue.AddListener(BeginDialogue);
            advanceDialogue.AddListener(AdvanceDialogue);
        }

        private void OnDisable()
        {
            beginDialogue.RemoveListener(BeginDialogue);
            advanceDialogue.RemoveListener(AdvanceDialogue);
        }

        private void BeginDialogue(DialogueAsset dialogueAsset)
        {
            if (currentDialogue != null) return;
            currentDialogue = dialogueAsset;
            
            AdvanceDialogue(new AdvanceDialogueContext());
        }

        private void AdvanceDialogue(AdvanceDialogueContext context)
        {
            do {
                currentDialogue.InvokeEvents();
                currentDialogue = currentDialogue.GetNextDialogue(context);
            } while (currentDialogue != null && currentDialogue is not IDialogueOutput);
            
            
            if (currentDialogue is IDialogueOutput outputDialogue)
            {
                displayDialogue.Invoke(outputDialogue.GetDialogueDetails());
            }
            else
            {
                endDialogue.Invoke();
            }
        }
    }
    

}
