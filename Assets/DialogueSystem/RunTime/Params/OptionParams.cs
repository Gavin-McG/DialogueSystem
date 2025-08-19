using System;
using System.ComponentModel;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public abstract class OptionParams
    {
        public string text;
        
        public string Text { get => text; set => text = value; }
        
        public abstract OptionParams Clone();
    }
}