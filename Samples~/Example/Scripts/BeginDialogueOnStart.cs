using UnityEngine;

namespace WolverineSoft.DialogueSystem.Example
{
    public class BeginDialogueOnStart : MonoBehaviour
    {
        [SerializeField] DialogueManager dialogueManager;
        [SerializeField] DialogueAsset dialogueAsset;

        private void Start()
        {
            dialogueManager.BeginDialogue(dialogueAsset);
        }
    }
}
