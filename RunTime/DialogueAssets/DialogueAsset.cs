using UnityEngine;
using UnityEngine.Serialization;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// ScriptableObject representing the main asset of a DialogueGraph and the first Trace when starting a dialogue
    /// </summary>
    public sealed class DialogueAsset : DialogueTrace
    {
        [SerializeField] public DialogueTrace nextDialogue;
        [SerializeField] public SettingsData settingsData;
        [SerializeField] public DialogueData endData = new();
        
        protected override DialogueTrace GetNextDialogue(AdvanceParams advanceParams, DialogueManager manager)
        {
            return nextDialogue;
        }

        public void RunEndOperations(DialogueManager manager)
        {
            endData.RunOperations(manager);
        }
    }

}
