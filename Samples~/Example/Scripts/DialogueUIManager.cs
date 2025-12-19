using UnityEngine;
using WolverineSoft.DialogueSystem.Default;


namespace WolverineSoft.DialogueSystem.Example
{
    /// <summary>
    /// Manage Interface between DialogueUI and DialogueManager
    /// </summary>
    public class DialogueUIManager : MonoBehaviour
    {
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private DialogueUI dialogueUI;
        
        private DialogueInfo currentDialogueInfo;

        private void OnEnable()
        {
            dialogueManager.StartedDialogue.AddListener(BeginDialogue);
            dialogueUI.AdvanceDialogue.AddListener(AdvanceDialogue);
        }

        private void OnDisable()
        {
            dialogueManager.StartedDialogue.RemoveListener(BeginDialogue);
            dialogueUI.AdvanceDialogue.RemoveListener(AdvanceDialogue);
        }

        private void BeginDialogue()
        {
            AdvanceDialogue(new AdvanceContext(-1));
        }

        public void AdvanceDialogue(AdvanceContext context)
        {
            DialogueInfo info = dialogueManager.AdvanceDialogue(context);

            if (info == null)
            {
                EndDialogue();
                return;
            }

            switch (info.dialogueType)
            {
                case DialogueType.Stall:
                    dialogueUI.ShowStallDialogue(info);
                    break;
                case DialogueType.Choice:
                    dialogueUI.ShowChoiceDialogue(info);
                    break;
                case DialogueType.Basic:
                    dialogueUI.ShowTextDialogue(info);
                    break;
            }
        }

        private void EndDialogue()
        {
            dialogueUI.EndDialogue();
        }
    }
}
