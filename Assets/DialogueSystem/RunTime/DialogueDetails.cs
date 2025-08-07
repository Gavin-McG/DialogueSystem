using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    public class DialogueDetails
    {
        public DialogueProfile profile;
        public string text;

        public bool isChoice;
        public List<string> choicePrompts;

        public bool hasTimeLimit;
        public float timeLimitDuration;
    }

}
