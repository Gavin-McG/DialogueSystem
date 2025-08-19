using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public abstract class BaseParams
    {
        public string text;
        
        public string Text { get => text; set => text = value; }

        public abstract BaseParams Clone();
    }
}