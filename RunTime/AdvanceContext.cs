using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Context that is passed into the DialogueManager to retrieve the next dialogue.
    /// Contains values relevant to the player's interaction and is used By redirects to determine next dialogue.
    /// An instance of AdvanceParams should be created by your frontend of the Dialogue System
    /// </summary>
    public class AdvanceContext
    {
        //Input choice selected (0 for non-choice dialogues)
        public int choice;

        public AdvanceContext()
        {
            choice = -1;
        }

        public AdvanceContext(int choice)
        {
            this.choice = choice;
        }
    }

}
