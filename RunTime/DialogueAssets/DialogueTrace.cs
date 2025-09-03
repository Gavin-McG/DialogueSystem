using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class for ScriptableObjects that are part of the path traced by the <see cref="DialogueManager"/>
    /// </summary>
    public abstract class DialogueTrace : ScriptableObject
    {
        [SerializeField] public DialogueData data = new();

        public DialogueTrace AdvanceDialogue(AdvanceContext context, DialogueManager manager)
        {
            data.RunOperations(manager);
            return GetNextDialogue(context, manager);
        }
        
        protected abstract DialogueTrace GetNextDialogue(AdvanceContext context, DialogueManager manager);
    }

}
