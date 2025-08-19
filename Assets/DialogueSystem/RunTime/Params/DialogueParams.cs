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
        public BaseParams baseParams;
        public ChoiceParams choiceParams;
        public List<OptionParams> choicePrompts = new();
        
        public DialogueParams() {}

        public DialogueParams(DialogueParams copyObj)
        {
            dialogueType = copyObj.dialogueType;
            baseParams = copyObj.baseParams?.GetCopy();
            choiceParams = copyObj.choiceParams?.GetCopy();
            choicePrompts = copyObj.choicePrompts;
        }

        public T GetBaseParams<T>() where T : BaseParams
        {
            if (baseParams is T tBaseParams) return tBaseParams;
            return null;
        }

        public T GetChoiceParams<T>() where T : ChoiceParams
        {
            if (choiceParams is T tChoiceParams) return tChoiceParams;
            return null;
        }
    }
    
}
