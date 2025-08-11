using System;
using System.ComponentModel;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class OptionParams
    {
        [DefaultValue("Response")]
        public string prompt;
    }
}