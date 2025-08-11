using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class DialogueGraphSettings
    {
        [Multiline, Tooltip("(Optional) description of dialogue purpose/contents")]
        public string dialogueDescription;
    }
}