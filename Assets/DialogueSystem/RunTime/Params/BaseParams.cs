using System;
using System.ComponentModel;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class BaseParams
    {
        public string text;
        public DialogueProfile profile;
    }
}