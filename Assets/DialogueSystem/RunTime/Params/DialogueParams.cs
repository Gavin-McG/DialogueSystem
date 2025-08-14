using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime
{

    [Serializable]
    public class DialogueParams
    {
        public enum DialogueType { Basic, Choice }
        
        public DialogueType dialogueType = DialogueType.Basic;
        public BaseParams baseParams = new BaseParams();
        public ChoiceParams choiceParams = new  ChoiceParams();
        public List<OptionParams> choicePrompts = new();
        
        public DialogueParams() {}

        public DialogueParams(DialogueParams copyObj)
        {
            dialogueType = copyObj.dialogueType;
            baseParams = new BaseParams(copyObj.baseParams);
            choiceParams = new ChoiceParams(copyObj.choiceParams);
            choicePrompts = copyObj.choicePrompts;
        }
    }
    
}
