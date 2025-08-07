using UnityEngine;

namespace DialogueSystem.Runtime
{

    public class AdvanceDialogueContext
    {
        //Time taken for user input
        public float inputDelay;
        //Input choice selected (0 for non-choice dialogues)
        public int choice = 0;
        //whether the player ran out of time on input
        public bool timedOut = false;
    }

}
