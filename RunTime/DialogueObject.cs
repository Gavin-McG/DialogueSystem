using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class for ScriptableObjects that are part of the path traced by the <see cref="DialogueManager"/>
    /// </summary>
    public abstract class DialogueObject : ScriptableObject
    {
        public abstract DialogueObject GetNextDialogue(AdvanceContext advanceContext, DialogueManager manager);
    }

}
