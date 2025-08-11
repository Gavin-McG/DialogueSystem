using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class DialogueBaseParams
    {
        public string text;
        public DialogueProfile profile;
    }

    [Serializable]
    public class DialogueChoiceParams
    {
        public bool hasTimeLimit;
        public float timeLimitDuration;
    }

    [Serializable]
    public class DialogueParams
    {
        public enum DialogueType { Basic, Choice }
        
        public DialogueType dialogueType;
        public DialogueBaseParams baseParams = new DialogueBaseParams();
        public DialogueChoiceParams choiceParams = null;
        public List<OptionParams> choicePrompts = new();
    }
}
