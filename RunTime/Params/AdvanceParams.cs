using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Context that is passed into the DialogueManager to retrieve the next dialogue.
    /// Contains values relevant to the player's interaction and is used By redirects to determine next dialogue.
    /// An instance of AdvanceParams should be created by your frontend of the Dialogue System
    /// </summary>
    public class AdvanceParams
    {
        //Input choice selected (0 for non-choice dialogues)
        public int choice;
        //whether the player ran out of time on input
        public bool timedOut;

        public AdvanceParams()
        {
            choice = 0;
            timedOut = false;
        }

        public AdvanceParams(int choice, bool timedOut)
        {
            this.choice = choice;
            this.timedOut = timedOut;
        }
    }

}
