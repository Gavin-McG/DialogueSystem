using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Context that is passed into the DialogueManager to retrieve the next dialogue.
    /// Contains values relevant to the player's interaction and is used By redirects to determine next dialogue.
    /// An instance of AdvanceDialogueContext should be created by your frontend of the Dialogue System
    /// </summary>
    public class AdvanceContext
    {
        //Time taken for user input
        public float inputDelay;
        //Input choice selected (0 for non-choice dialogues)
        public int choice;
        //whether the player ran out of time on input
        public bool timedOut;

        public AdvanceContext()
        {
            inputDelay = 0;
            choice = 0;
            timedOut = false;
        }

        public AdvanceContext(float inputDelay, int choice, bool timedOut)
        {
            this.inputDelay = inputDelay;
            this.choice = choice;
            this.timedOut = timedOut;
        }
    }

}
