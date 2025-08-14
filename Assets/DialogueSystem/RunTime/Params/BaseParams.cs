using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class BaseParams
    {
        public string text;
        public DialogueProfile profile;
        
        public string Text { get => text; set => text = value; }

        public BaseParams()
        {
            text = string.Empty;
            profile = null;
        }
        
        public BaseParams(BaseParams copyObj)
        {
            text = new string(copyObj.text);
            profile = copyObj.profile;
        }
    }
}