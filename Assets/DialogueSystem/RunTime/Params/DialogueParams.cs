using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    [Serializable]
    public class DialogueParams
    {
        public enum DialogueType { Basic, Choice }
        
        public DialogueType dialogueType;
        public BaseParams baseParams = new BaseParams();
        public ChoiceParams choiceParams = null;
        public List<OptionParams> choicePrompts = new();
    }
    
}
